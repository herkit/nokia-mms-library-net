using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NokiaMMSLibraryNet
{
    public class MMEncoder
    {
        private const byte FIELDBASE = 0x80;
        private const byte TRUE = 0x80;
        private const byte FALSE = 0x81;
        private const int CHARSET_PARAMETER = 0x81;
        private MMMessage m_Message;
        private bool m_bMessageAvailable;
        private bool m_bMessageEcoded;
        private MemoryStream m_Out;

        public MMEncoder()
        {
            Reset();
        }

        /**  
         * Resets the Decoder object.  
         *  
         */
        public void Reset()
        {
            m_Message = null;
            m_bMessageAvailable = false;
            m_bMessageEcoded = false;
            m_Out = null;
        }

        /**  
         * Sets the Multimedia Message to be encoded.  
         */
        public void SetMessage(MMMessage msg)
        {
            m_Message = msg;
            m_bMessageAvailable = true;
        }

        /**  
         * Retrieve the buffer of byte representing the encoded Multimedia Message.  
         * This method has to be called after the calling to encodeMessasge()  
         *  
         * @return the array of bytes representing the Multmedia Message  
         */
        public byte[] GetMessage()
        {
            if (m_bMessageEcoded)
            {
                return m_Out.ToArray();
            }
            else
            {
                return null;
            }
        }
        /**  
         * Encode known content type assignments.  
         * List of the content type assignments can be found from WAP-203-WSP, Table 40  
         * This version is compliant with Approved version 4-May-2000  
         *  
         * @return assigned number  
         */

        private static byte EncodeContentType(string sContentType)
        {
            Dictionary<string, byte> contentTypeMap = new Dictionary<string, byte> {
                            { "*/*", 0x00 },
                            { "text/*", 0x01 },
                            { "text/html", 0x02 },
                            { "text/plain", 0x03 },
                            { "text/x-hdml", 0x04 },
                            { "text/x-ttml", 0x05 },
                            { "text/x-vCalendar", 0x06 },
                            { "text/x-vCard", 0x07 },
                            { "text/vnd.wap.wml", 0x08 },
                            { "text/vnd.wap.wmlscript", 0x09 },
                            { "text/vnd.wap.channel", 0x0A },
                            { "multipart/*", 0x0B },
                            { "multipart/mixed", 0x0C },
                            { "multipart/form-data", 0x0D },
                            { "multipart/byteranges", 0x0E },
                            { "multipart/alternative", 0x0F },
                            { "application/*", 0x10 },
                            { "application/java-vm", 0x11 },
                            { "application/x-www-form-urlencoded", 0x12 },
                            { "application/x-hdmlc", 0x13 },
                            { "application/vnd.wap.wmlc", 0x14 },
                            { "application/vnd.wap.wmlscriptc", 0x15 },
                            { "application/vnd.wap.channelc", 0x16 },
                            { "application/vnd.wap.uaprof", 0x17 },
                            { "application/vnd.wap.wtls-ca-certificate", 0x18 },
                            { "application/vnd.wap.wtls-user-certificate", 0x19 },
                            { "application/x-x509-ca-cert", 0x1A },
                            { "application/x-x509-user-cert", 0x1B },
                            { "image/*", 0x1C },
                            { "image/gif", 0x1D },
                            { "image/jpeg", 0x1E },
                            { "image/tiff", 0x1F },
                            { "image/png", 0x20 },
                            { "image/vnd.wap.wbmp", 0x21 },
                            { "application/vnd.wap.multipart.*", 0x22 },
                            { "application/vnd.wap.multipart.mixed", 0x23 },
                            { "application/vnd.wap.multipart.form-data", 0x24 },
                            { "application/vnd.wap.multipart.byteranges", 0x25 },
                            { "application/vnd.wap.multipart.alternative", 0x26 },
                            { "application/xml", 0x27 },
                            { "text/xml", 0x28 },
                            { "application/vnd.wap.wbxml", 0x29 },
                            { "application/x-x968-cross-cert", 0x2A },
                            { "application/x-x968-ca-cert", 0x2B },
                            { "application/x-x968-user-cert", 0x2C },
                            { "text/vnd.wap.si", 0x2D },
                            { "application/vnd.wap.sic", 0x2E },
                            { "text/vnd.wap.sl", 0x2F },
                            { "application/vnd.wap.slc", 0x30 },
                            { "text/vnd.wap.co", 0x31 },
                            { "application/vnd.wap.coc", 0x32 },
                            { "application/vnd.wap.multipart.related", 0x33 },
                            { "application/vnd.wap.sia", 0x34 },
                            { "text/vnd.wap.connectivity-xml", 0x35 },
                            { "application/vnd.wap.connectivity-wbxml", 0x36 }
                        };

            if (contentTypeMap.ContainsKey(sContentType.ToLower()))
                return contentTypeMap[sContentType.ToLower()];
            else
                return 0;
        }

        private static void WriteValueLength(long value, MMBinaryWriter writer)
        {

            if (value <= 30)
                writer.Write((byte)value);
            else
            {
                writer.Write((byte)31);
                var data = EncodeUintvarNumber(value);
                byte numValue;
                for (int i = 1; i <= data[0]; i++)
                {
                    numValue = data[i];
                    writer.Write(numValue);
                }
            }
        }

        private static void WriteUintvar(long value, MMBinaryWriter writer)
        {
            var data = EncodeUintvarNumber(value);
            byte numValue;
            for (int i = 1; i <= data[0]; i++)
            {
                numValue = data[i];
                writer.Write(numValue);
            }
        }

        public void EncodeMessage(Stream output)
        {
            int numValue;
            String strValue;
            try
            {
                bool m_bMultipartRelated = false;

                using (MMBinaryWriter sw_Out = new MMBinaryWriter(output, System.Text.Encoding.UTF8, true))
                {
                    if (!m_Message.IsMessageTypeAvailable)
                    {
                        sw_Out.Close();
                        throw new MMEncoderException("Invalid Multimedia Message format.");
                    }
                    byte nMessageType = m_Message.MessageType;
                    switch (nMessageType)
                    {
                        case MMConstants.MESSAGE_TYPE_M_DELIVERY_IND // ---------------------------- m-delivery-ind
                        :
                            // ------------------- MESSAGE TYPE --------
                            sw_Out.Write(((byte)MMConstants.FN_MESSAGE_TYPE + FIELDBASE));
                            sw_Out.Write(nMessageType);
                            // ------------------- MESSAGE ID ------
                            if (m_Message.IsMessageIdAvailable)
                            {
                                sw_Out.Write(((byte)MMConstants.FN_MESSAGE_ID + FIELDBASE));
                                sw_Out.Write(m_Message.MessageId);
                            }
                            else
                            {
                                sw_Out.Close();
                                throw new MMEncoderException("The field Message-ID of the Multimedia Message is null");
                            }
                            // ------------------- VERSION -------------
                            sw_Out.Write(((byte)MMConstants.FN_MMS_VERSION + FIELDBASE));
                            if (!m_Message.IsVersionAvailable)
                            {
                                numValue = MMConstants.MMS_VERSION_10;
                            }
                            else
                            {
                                numValue = m_Message.Version;
                            }
                            sw_Out.Write(numValue);
                            // ------------------- DATE ----------------
                            if (m_Message.IsDateAvailable)
                            {
                                long secs = m_Message.Date.ToUniversalTime().TotalSeconds();
                                var data = EncodeMultiByteNumber(secs);
                                if (data == null)
                                {
                                    sw_Out.Close();
                                    throw new MMEncoderException("An error occurred encoding the sending date of the Multimedia Message");
                                }
                                sw_Out.Write(((byte)MMConstants.FN_DATE + FIELDBASE));
                                int nCount = data[0];
                                sw_Out.Write(nCount);
                                for (int i = 1; i <= nCount; i++)
                                {
                                    sw_Out.Write(data[i]);
                                }
                            }
                            // ------------------- TO ------------------
                            if (m_Message.IsToAvailable)
                            {
                                var sAddress = m_Message.To;
                                int nAddressCount = sAddress.Count;
                                if (sAddress == null)
                                {
                                    sw_Out.Close();
                                    throw new MMEncoderException("The field TO of the Multimedia Message is set to null.");
                                }
                                for (int i = 0; i < nAddressCount; i++)
                                {
                                    strValue = ((MMAddress)sAddress[i]).FullAddress;
                                    if (strValue != null)
                                    {
                                        sw_Out.Write(((byte)MMConstants.FN_TO + FIELDBASE));
                                        sw_Out.Write(strValue);
                                    }
                                }
                            }
                            else
                            {
                                sw_Out.Close();
                                throw new MMEncoderException("No recipient specified in the Multimedia Message.");
                            }
                            // ------------------- MESSAGE-STATUS ----------------
                            if (m_Message.IsStatusAvailable)
                            {
                                sw_Out.Write(((byte)MMConstants.FN_STATUS + FIELDBASE));
                                sw_Out.Write(m_Message.MessageStatus);
                            }
                            else
                            {
                                sw_Out.Close();
                                throw new MMEncoderException("The field Message-ID of the Multimedia Message is null");
                            }
                            break;
                        case MMConstants.MESSAGE_TYPE_M_SEND_REQ // ---------------------------- m-send-req
                        :
                            // ------------------- MESSAGE TYPE --------
                            sw_Out.Write((byte)(MMConstants.FN_MESSAGE_TYPE + FIELDBASE));
                            sw_Out.Write(nMessageType);
                            // ------------------- TRANSACTION ID ------
                            if (m_Message.IsTransactionIdAvailable)
                            {
                                sw_Out.Write((byte)(MMConstants.FN_TRANSACTION_ID + FIELDBASE));
                                sw_Out.Write(m_Message.TransactionId);
                            }
                            // ------------------- VERSION -------------
                            sw_Out.Write((byte)(MMConstants.FN_MMS_VERSION + FIELDBASE));
                            if (!m_Message.IsVersionAvailable)
                            {
                                numValue = MMConstants.MMS_VERSION_10;
                            }
                            else
                            {
                                numValue = m_Message.Version;
                            }
                            sw_Out.Write((byte)numValue);
                            // ------------------- DATE ----------------
                            if (m_Message.IsDateAvailable)
                            {
                                long secs = m_Message.Date.ToUniversalTime().TotalSeconds();
                                var data = EncodeMultiByteNumber(secs);
                                if (data == null)
                                {
                                    sw_Out.Close();
                                    throw new MMEncoderException("An error occurred encoding the sending date of the Multimedia Message");
                                }
                                sw_Out.Write((byte)(MMConstants.FN_DATE + FIELDBASE));
                                byte nCount = data[0];
                                sw_Out.Write(nCount);
                                for (byte i = 1; i <= nCount; i++)
                                {
                                    sw_Out.Write(data[i]);
                                }
                            }
                            // ------------------- FROM ----------------
                            if (m_Message.IsFromAvailable)
                            {
                                sw_Out.Write((byte)(MMConstants.FN_FROM + FIELDBASE));
                                strValue = m_Message.From.FullAddress;
                                if (strValue == null)
                                {
                                    sw_Out.Close();
                                    throw new MMEncoderException("The field from is assigned to null");
                                }
                                // Value-length
                                WriteValueLength(strValue.Length + 2, sw_Out);
                                // Address-present-token
                                sw_Out.Write((byte)TRUE);
                                // Encoded-string-value
                                sw_Out.Write(strValue);
                            }
                            else
                            {
                                // Value-length
                                sw_Out.Write((byte)1);
                                sw_Out.Write((byte)FALSE);
                            }
                            // ------------------- TO ------------------
                            if (m_Message.IsToAvailable)
                            {
                                List<MMAddress> sAddress = m_Message.To;
                                int nAddressCount = sAddress.Count;
                                if (sAddress == null)
                                {
                                    sw_Out.Close();
                                    throw new MMEncoderException("The field TO of the Multimedia Message is set to null.");
                                }
                                for (int i = 0; i < nAddressCount; i++)
                                {
                                    strValue = ((MMAddress)sAddress[i]).FullAddress;
                                    if (strValue != null)
                                    {
                                        sw_Out.Write((byte)(MMConstants.FN_TO + FIELDBASE));
                                        sw_Out.Write(strValue);
                                    }
                                }
                            }
                            // ------------------- CC ------------------
                            if (m_Message.IsCcAvailable)
                            {
                                List<MMAddress> sAddress = m_Message.Cc;
                                int nAddressCount = sAddress.Count;
                                if (sAddress == null)
                                {
                                    sw_Out.Close();
                                    throw new MMEncoderException("The field CC of the Multimedia Message is set to null.");
                                }
                                for (int i = 0; i < nAddressCount; i++)
                                {
                                    strValue = ((MMAddress)sAddress[i]).FullAddress;
                                    if (strValue != null)
                                    {
                                        sw_Out.Write((byte)(MMConstants.FN_CC + FIELDBASE));
                                        sw_Out.Write(strValue);
                                    }
                                }
                            }
                            // ------------------- BCC ------------------
                            if (m_Message.IsBccAvailable)
                            {
                                List<MMAddress> sAddress = m_Message.Bcc;
                                int nAddressCount = sAddress.Count;
                                if (sAddress == null)
                                {
                                    sw_Out.Close();
                                    throw new MMEncoderException("The field BCC of the Multimedia Message is set to null.");
                                }
                                for (int i = 0; i < nAddressCount; i++)
                                {
                                    strValue = ((MMAddress)sAddress[i]).FullAddress;
                                    if (strValue != null)
                                    {
                                        sw_Out.Write((byte)(MMConstants.FN_BCC + FIELDBASE));
                                        sw_Out.Write(strValue);
                                    }
                                }
                            }
                            if (!(m_Message.IsToAvailable || m_Message.IsCcAvailable || m_Message.IsBccAvailable))
                            {
                                sw_Out.Close();
                                throw new MMEncoderException("No recipient specified in the Multimedia Message.");
                            }
                            // ---------------- SUBJECT  --------------
                            if (m_Message.IsSubjectAvailable)
                            {
                                sw_Out.Write((byte)(MMConstants.FN_SUBJECT + FIELDBASE));
                                if (m_Message.IncludeEncodingInSubject)
                                {
                                    sw_Out.Write((byte)((m_Message.Subject.Length + 2) % 256));
                                    sw_Out.Write((byte)0xEA);
                                }
                                sw_Out.Write(m_Message.Subject);
                            }
                            // ------------------- DELIVERY-REPORT ----------------
                            if (m_Message.IsDeliveryReportAvailable)
                            {
                                sw_Out.Write((byte)(MMConstants.FN_DELIVERY_REPORT + FIELDBASE));
                                if (m_Message.DeliveryReport == true)
                                    sw_Out.Write(TRUE);
                                else
                                    sw_Out.Write(FALSE);
                            }
                            // ------------------- SENDER-VISIBILITY ----------------
                            if (m_Message.IsSenderVisibilityAvailable)
                            {
                                sw_Out.Write((byte)(MMConstants.FN_SENDER_VISIBILITY + FIELDBASE));
                                sw_Out.Write(m_Message.SenderVisibility);
                            }
                            // ------------------- READ-REPLY ----------------
                            if (m_Message.IsReadReplyAvailable)
                            {
                                sw_Out.Write((byte)(MMConstants.FN_READ_REPLY + FIELDBASE));
                                if (m_Message.ReadReply == true)
                                    sw_Out.Write(TRUE);
                                else
                                    sw_Out.Write(FALSE);
                            }
                            // ---------------- MESSAGE CLASS ---------
                            if (m_Message.IsMessageClassAvailable)
                            {
                                sw_Out.Write((byte)(MMConstants.FN_MESSAGE_CLASS + FIELDBASE));
                                sw_Out.Write(m_Message.MessageClass);
                            }
                            // ---------------- EXPIRY ----------------
                            if (m_Message.IsExpiryAvailable)
                            {
                                long secs = m_Message.Expiry.ToUniversalTime().TotalSeconds();
                                var data = EncodeMultiByteNumber(secs);
                                if (data == null)
                                {
                                    sw_Out.Close();
                                    throw new MMEncoderException("An error occurred encoding the EXPIRY field of the Multimedia Message. The field is set to null");
                                }
                                int nCount = data[0];
                                sw_Out.Write((byte)(MMConstants.FN_EXPIRY + FIELDBASE));
                                // Value-length
                                WriteValueLength(nCount + 2, sw_Out);
                                if (m_Message.IsExpiryAbsolute)
                                {
                                    // Absolute-token
                                    sw_Out.Write(TRUE);
                                }
                                else
                                {
                                    // Relative-token
                                    sw_Out.Write(FALSE);
                                }
                                // Date-value or Delta-seconds-value
                                for (int i = 0; i <= nCount; i++)
                                {
                                    sw_Out.Write(data[i]);
                                }
                            }
                            // ---------------- DELIVERY TIME ----------------
                            if (m_Message.IsDeliveryTimeAvailable)
                            {
                                long secs = m_Message.DeliveryTime.ToUniversalTime().TotalSeconds();
                                var data = EncodeMultiByteNumber(secs);
                                if (data == null)
                                {
                                    sw_Out.Close();
                                    throw new MMEncoderException("The field DELIVERY TIME of the Multimedia Message is set to null.");
                                }
                                int nCount = data[0];
                                sw_Out.Write((byte)(MMConstants.FN_DELIVERY_TIME + FIELDBASE));
                                // Value-length
                                WriteValueLength(nCount + 2, sw_Out);
                                if (m_Message.IsDeliveryTimeAbsolute)
                                {
                                    // Absolute-token
                                    sw_Out.Write((byte)TRUE);
                                }
                                else
                                {
                                    // Relative-token
                                    sw_Out.Write((byte)FALSE);
                                }
                                // Date-value or Delta-seconds-value
                                for (int i = 0; i <= nCount; i++)
                                {
                                    sw_Out.Write(data[i]);
                                }
                            }
                            // ---------------- PRIORITY ----------------
                            if (m_Message.IsPriorityAvailable)
                            {
                                sw_Out.Write((byte)(MMConstants.FN_PRIORITY + FIELDBASE));
                                sw_Out.Write(m_Message.Priority);
                            }
                            // ---------------- CONTENT TYPE ----------------
                            if (m_Message.IsContentTypeAvailable)
                            {
                                m_bMultipartRelated = false;
                                sw_Out.Write((byte)(MMConstants.FN_CONTENT_TYPE + FIELDBASE));
                                byte ctype = EncodeContentType(m_Message.ContentType);
                                if (ctype == 0x33)
                                {
                                    // application/vnd.wap.multipart.related
                                    m_bMultipartRelated = true;
                                    if (!string.IsNullOrWhiteSpace(m_Message.MultipartRelatedType))
                                    {
                                        int valueLength = 1;
                                        String mprt = m_Message.MultipartRelatedType;
                                        valueLength += mprt.Length + 2;
                                        String start = m_Message.PresentationId;
                                        valueLength += start.Length + 2;
                                        // Value-length
                                        WriteValueLength(valueLength, sw_Out);
                                        // Well-known-media
                                        sw_Out.Write((byte)(0x33 + FIELDBASE));
                                        // Parameters
                                        // Type
                                        sw_Out.Write((byte)(0x09 + FIELDBASE));
                                        sw_Out.Write(mprt);
                                        // Start
                                        sw_Out.Write((byte)(0x0A + FIELDBASE));
                                        sw_Out.Write(start);
                                        //sw_Out.Write(0x00);
                                    }
                                    else
                                    {
                                        sw_Out.Write((byte)(ctype + FIELDBASE));
                                    }
                                }
                                else
                                {
                                    if (ctype > 0x00)
                                        sw_Out.Write((byte)(ctype + FIELDBASE));
                                    else
                                    {
                                        sw_Out.Write(m_Message.ContentType);
                                    }
                                }
                            }
                            else
                            {
                                sw_Out.Close();
                                throw new MMEncoderException("The field CONTENT TYPE of the Multimedia Message is not specified.");
                            }
                            // -------------------------- BODY -------------
                            byte nPartsCount = (byte)m_Message.NumContents;
                            sw_Out.Write(nPartsCount);
                            MMContent part = null;
                            for (int i = 0; i < nPartsCount; i++)
                            {
                                part = m_Message.GetContent(i);
                                bool bRetVal = EncodePart(part, sw_Out, m_bMultipartRelated);
                                if (!bRetVal)
                                {
                                    sw_Out.Close();
                                    throw new MMEncoderException("The entry having Content-id = " + part.ContentId + " cannot be encoded.");
                                }
                            }
                            break;
                        default:
                            {
                                sw_Out.Close();
                                throw new MMEncoderException("Invalid Multimedia Message format.");
                            }
                    }
                }

                //sw_Out.Close();
                m_bMessageEcoded = true;
            }
            catch (IOException e)
            {
                throw new MMEncoderException("An IO error occurred encoding the Multimedia Message.");
            }
        }
        /**   
                 * Encodes the Multimedia Message set by calling setMessage(MMMessage msg)   
                 */
        public void EncodeMessage()
        {
            m_Out = new MemoryStream();

            int numValue;
            String strValue;
            m_bMessageEcoded = false;

            if (!m_bMessageAvailable)
                throw new MMEncoderException("No Multimedia Messages set in the encoder");

            EncodeMessage(m_Out);
        }

        private static byte[] EncodeMultiByteNumber(long lData)
        {

            var data = new byte[32];
            long lDivider = 1L;
            byte nSize = 0;
            long lNumber = lData;

            for (int i = 0; i < 32; i++)
                data[i] = 0;

            for (int i = 4; i >= 0; i--)
            {
                lDivider = 1L;
                for (int j = 0; j < i; j++)
                    lDivider *= 256L;

                byte q = (byte)(lNumber / lDivider % 256);

                if (q != 0 || nSize != 0)
                {
                    long r = (long)(lNumber % lDivider);
                    data[nSize + 1] = q;
                    lNumber = r;
                    nSize++;
                }
            }

            data[0] = nSize;
            return data;
        }

        private static byte[] EncodeUintvarNumber(long lData)
        {
            var data = new byte[32];
            long lDivider = 1L;
            byte nSize = 0;
            long lNumber = lData;

            for (int i = 0; i < 32; i++)
                data[i] = 0;

            for (int i = 4; i >= 0; i--)
            {
                lDivider = 1L;
                for (int j = 0; j < i; j++)
                    lDivider *= 128L;

                byte q = (byte)(lNumber / lDivider % 256);
                if (q != 0 || nSize != 0)
                {
                    long r = (long)(lNumber % lDivider);
                    data[nSize + 1] = q;
                    if (i != 0)
                        data[nSize + 1] += 128;
                    lNumber = r;
                    nSize++;
                }
            }

            data[0] = nSize;
            return data;
        }

        private static bool EncodePart(MMContent part, MMBinaryWriter writer, bool messageIsMultipartRelated)
        {

            if (part == null)
                return false;

            int nHeadersLen = 0; // nHeadersLen = nLengthOfContentType + nLengthOfHeaders   
            int nContentType = 0;

            int nLengthOfHeaders = 0;
            int nLengthOfContentType = 0;


            // -------- HeadersLen = ContentType + Headers fields ---------   
            if ((part.ContentId.Length > 0) && (messageIsMultipartRelated))
            {
                if (part.ContentId[0] == '<')
                {
                    nLengthOfHeaders = 2 + (part.ContentId).Length + 1;
                    // 2 = 0xC0 (Content-ID) + 0x22 (quotes)   
                    // 1 = 0x00 (at the end of the contentID)   
                }
                else
                {
                    nLengthOfHeaders = 1 + part.ContentId.Length + 1;
                    // 1 = 0xC0 (Content-Location)   
                    // 1 = 0x00 (end string)   
                }
            }

            if (part.IsContentLocationAvailable)
            {
                nLengthOfHeaders += 1 + part.ContentLocation.Length + 1;
                // 1 = 0x8E (Content-Location)   
                // 1 = 0x00 (end string)   
            }

            // -------- DataLen -------------   
            long lDataLen = part.Length;

            // -------- ContentType ---------   
            nContentType = EncodeContentType(part.Type) + FIELDBASE;

            if (nContentType > FIELDBASE)
            {
                // ---------- Well Known Content Types ------------------------------   
                if (nContentType == 0x83)
                { // text/plain   
                    nLengthOfContentType = 4;
                    // 4 = 0x03 (Value Length)+ 0x83(text/plain) + 0x81 (Charset) + 0x83 (us-ascii code)   

                    nHeadersLen = nLengthOfContentType + nLengthOfHeaders;

                    // Write HeadersLen   
                    WriteUintvar(nHeadersLen, writer);

                    // Write DataLen   
                    WriteUintvar(lDataLen, writer);

                    // Write ContentType   
                    writer.Write((byte)0x03); // length of content type   
                    writer.Write((byte)nContentType);
                    writer.Write((byte)CHARSET_PARAMETER); // charset parameter   
                    writer.Write((byte)0xEA); // us-ascii code   
                }
                else
                {
                    nLengthOfContentType = 1;
                    nHeadersLen = nLengthOfContentType + nLengthOfHeaders;
                    // Write HeadersLen   
                    WriteUintvar(nHeadersLen, writer);
                    // Write DataLen   
                    WriteUintvar(lDataLen, writer);
                    // Write ContentType   
                    writer.Write((byte)nContentType);
                }
            }
            else
            {
                // ----------- Don't known Content Type   
                if (part.Type.Equals(MMConstants.CT_APPLICATION_SMIL, StringComparison.InvariantCultureIgnoreCase))
                {
                    nLengthOfContentType = 1 + part.Type.Length + 3;
                    // 1 = 0x13 (Value Length)   
                    // 3 = 0x00 + 0x81 (Charset) + 0x83 (us-ascii code)   

                    nHeadersLen = nLengthOfContentType + nLengthOfHeaders;

                    // Write HeadersLen   
                    WriteUintvar(nHeadersLen, writer);
                    // Write DataLen   
                    WriteUintvar(lDataLen, writer);

                    // Write ContentType   
                    writer.Write((byte)0x13); //13 characters, actually part.getType().Length+1+1+1   
                    writer.Write(part.Type);
                    //sw_Out.Write(0x00);   
                    writer.Write((byte)FALSE); // charset parameter   
                    writer.Write((byte)0xEA); // ascii-code   
                }
                else
                {
                    nLengthOfContentType = part.Type.Length + 1;
                    // 1 = 0x00   

                    nHeadersLen = nLengthOfContentType + nLengthOfHeaders;

                    // Write HeadersLen   
                    WriteUintvar(nHeadersLen, writer);
                    // Write DataLen   
                    WriteUintvar(lDataLen, writer);
                    // Write ContentType   
                    writer.Write(part.Type);
                    //sw_Out.Write(0x00);   
                }
            }

            if (part.IsContentLocationAvailable)
            {
                // content id   
                writer.Write((byte)0x8E);
                writer.Write(part.ContentLocation);
                //sw_Out.Write(0x00);   
            }

            // Writes the Content ID or the Content Location   
            if ((part.ContentId.Length > 0) && (messageIsMultipartRelated))
            {
                writer.Write((byte)0xC0);
                if (part.ContentId.First() == '<')
                {
                    Console.Out.WriteLine("--->QUOTED!!");
                    writer.Write((byte)0x22);
                }
                writer.Write(part.ContentId);
            }

            // ----------- Data --------------   
            byte[] data;
            data = part.GetContent();
            writer.Write(data);

            return true;
        }
    }
}
