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
        private bool m_bMultipartRelated;
        private bool m_bMessageEcoded;
        private MemoryStream m_Out;
        private MMBinaryWriter sw_Out;

        public MMEncoder()
        {
            reset();
        }

        /**  
         * Resets the Decoder object.  
         *  
         */
        public void reset()
        {
            m_Message = null;
            m_bMultipartRelated = false;
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

        private byte EncodeContentType(string sContentType)
        {
            if (sContentType.Equals("*/*", StringComparison.InvariantCultureIgnoreCase))
                return 0x00;
            else
                if (sContentType.Equals("text/*", StringComparison.InvariantCultureIgnoreCase))
                    return 0x01;
                else
                    if (sContentType.Equals("text/html", StringComparison.InvariantCultureIgnoreCase))
                        return 0x02;
                    else
                        if (sContentType.Equals("text/plain", StringComparison.InvariantCultureIgnoreCase))
                            return 0x03;
                        else
                            if (sContentType.Equals("text/x-hdml", StringComparison.InvariantCultureIgnoreCase))
                                return 0x04;
                            else
                                if (sContentType.Equals("text/x-ttml", StringComparison.InvariantCultureIgnoreCase))
                                    return 0x05;
                                else
                                    if (sContentType.Equals("text/x-vCalendar", StringComparison.InvariantCultureIgnoreCase))
                                        return 0x06;
                                    else
                                        if (sContentType.Equals("text/x-vCard", StringComparison.InvariantCultureIgnoreCase))
                                            return 0x07;
                                        else
                                            if (sContentType.Equals("text/vnd.wap.wml", StringComparison.InvariantCultureIgnoreCase))
                                                return 0x08;
                                            else
                                                if (sContentType.Equals("text/vnd.wap.wmlscript", StringComparison.InvariantCultureIgnoreCase))
                                                    return 0x09;
                                                else
                                                    if (sContentType.Equals("text/vnd.wap.channel", StringComparison.InvariantCultureIgnoreCase))
                                                        return 0x0A;
                                                    else
                                                        if (sContentType.Equals("multipart/*", StringComparison.InvariantCultureIgnoreCase))
                                                            return 0x0B;
                                                        else
                                                            if (sContentType.Equals("multipart/mixed", StringComparison.InvariantCultureIgnoreCase))
                                                                return 0x0C;
                                                            else
                                                                if (sContentType.Equals("multipart/form-data", StringComparison.InvariantCultureIgnoreCase))
                                                                    return 0x0D;
                                                                else
                                                                    if (sContentType.Equals("multipart/byteranges", StringComparison.InvariantCultureIgnoreCase))
                                                                        return 0x0E;
                                                                    else
                                                                        if (sContentType.Equals("multipart/alternative", StringComparison.InvariantCultureIgnoreCase))
                                                                            return 0x0F;
                                                                        else
                                                                            if (sContentType.Equals("application/*", StringComparison.InvariantCultureIgnoreCase))
                                                                                return 0x10;
                                                                            else
                                                                                if (sContentType.Equals("application/java-vm", StringComparison.InvariantCultureIgnoreCase))
                                                                                    return 0x11;
                                                                                else
                                                                                    if (sContentType.Equals("application/x-www-form-urlencoded", StringComparison.InvariantCultureIgnoreCase))
                                                                                        return 0x12;
                                                                                    else
                                                                                        if (sContentType.Equals("application/x-hdmlc", StringComparison.InvariantCultureIgnoreCase))
                                                                                            return 0x13;
                                                                                        else
                                                                                            if (sContentType.Equals("application/vnd.wap.wmlc", StringComparison.InvariantCultureIgnoreCase))
                                                                                                return 0x14;
                                                                                            else
                                                                                                if (sContentType.Equals("application/vnd.wap.wmlscriptc", StringComparison.InvariantCultureIgnoreCase))
                                                                                                    return 0x15;
                                                                                                else
                                                                                                    if (sContentType.Equals("application/vnd.wap.channelc", StringComparison.InvariantCultureIgnoreCase))
                                                                                                        return 0x16;
                                                                                                    else
                                                                                                        if (sContentType.Equals("application/vnd.wap.uaprof", StringComparison.InvariantCultureIgnoreCase))
                                                                                                            return 0x17;
                                                                                                        else
                                                                                                            if (sContentType.Equals("application/vnd.wap.wtls-ca-certificate", StringComparison.InvariantCultureIgnoreCase))
                                                                                                                return 0x18;
                                                                                                            else
                                                                                                                if (sContentType.Equals("application/vnd.wap.wtls-user-certificate", StringComparison.InvariantCultureIgnoreCase))
                                                                                                                    return 0x19;
                                                                                                                else
                                                                                                                    if (sContentType.Equals("application/x-x509-ca-cert", StringComparison.InvariantCultureIgnoreCase))
                                                                                                                        return 0x1A;
                                                                                                                    else
                                                                                                                        if (sContentType.Equals("application/x-x509-user-cert", StringComparison.InvariantCultureIgnoreCase))
                                                                                                                            return 0x1B;
                                                                                                                        else
                                                                                                                            if (sContentType.Equals("image/*", StringComparison.InvariantCultureIgnoreCase))
                                                                                                                                return 0x1C;
                                                                                                                            else
                                                                                                                                if (sContentType.Equals("image/gif", StringComparison.InvariantCultureIgnoreCase))
                                                                                                                                    return 0x1D;
                                                                                                                                else
                                                                                                                                    if (sContentType.Equals("image/jpeg", StringComparison.InvariantCultureIgnoreCase))
                                                                                                                                        return 0x1E;
                                                                                                                                    else
                                                                                                                                        if (sContentType.Equals("image/tiff", StringComparison.InvariantCultureIgnoreCase))
                                                                                                                                            return 0x1F;
                                                                                                                                        else
                                                                                                                                            if (sContentType.Equals("image/png", StringComparison.InvariantCultureIgnoreCase))
                                                                                                                                                return 0x20;
                                                                                                                                            else
                                                                                                                                                if (sContentType.Equals("image/vnd.wap.wbmp", StringComparison.InvariantCultureIgnoreCase))
                                                                                                                                                    return 0x21;
                                                                                                                                                else
                                                                                                                                                    if (sContentType.Equals("application/vnd.wap.multipart.*", StringComparison.InvariantCultureIgnoreCase))
                                                                                                                                                        return 0x22;
                                                                                                                                                    else
                                                                                                                                                        if (sContentType.Equals("application/vnd.wap.multipart.mixed", StringComparison.InvariantCultureIgnoreCase))
                                                                                                                                                            return 0x23;
                                                                                                                                                        else
                                                                                                                                                            if (sContentType.Equals("application/vnd.wap.multipart.form-data", StringComparison.InvariantCultureIgnoreCase))
                                                                                                                                                                return 0x24;
                                                                                                                                                            else
                                                                                                                                                                if (sContentType.Equals("application/vnd.wap.multipart.byteranges", StringComparison.InvariantCultureIgnoreCase))
                                                                                                                                                                    return 0x25;
                                                                                                                                                                else
                                                                                                                                                                    if (sContentType.Equals("application/vnd.wap.multipart.alternative", StringComparison.InvariantCultureIgnoreCase))
                                                                                                                                                                        return 0x26;
                                                                                                                                                                    else
                                                                                                                                                                        if (sContentType.Equals("application/xml", StringComparison.InvariantCultureIgnoreCase))
                                                                                                                                                                            return 0x27;
                                                                                                                                                                        else
                                                                                                                                                                            if (sContentType.Equals("text/xml", StringComparison.InvariantCultureIgnoreCase))
                                                                                                                                                                                return 0x28;
                                                                                                                                                                            else
                                                                                                                                                                                if (sContentType.Equals("application/vnd.wap.wbxml", StringComparison.InvariantCultureIgnoreCase))
                                                                                                                                                                                    return 0x29;
                                                                                                                                                                                else
                                                                                                                                                                                    if (sContentType.Equals("application/x-x968-cross-cert", StringComparison.InvariantCultureIgnoreCase))
                                                                                                                                                                                        return 0x2A;
                                                                                                                                                                                    else
                                                                                                                                                                                        if (sContentType.Equals("application/x-x968-ca-cert", StringComparison.InvariantCultureIgnoreCase))
                                                                                                                                                                                            return 0x2B;
                                                                                                                                                                                        else
                                                                                                                                                                                            if (sContentType.Equals("application/x-x968-user-cert", StringComparison.InvariantCultureIgnoreCase))
                                                                                                                                                                                                return 0x2C;
                                                                                                                                                                                            else
                                                                                                                                                                                                if (sContentType.Equals("text/vnd.wap.si", StringComparison.InvariantCultureIgnoreCase))
                                                                                                                                                                                                    return 0x2D;
                                                                                                                                                                                                else
                                                                                                                                                                                                    if (sContentType.Equals("application/vnd.wap.sic", StringComparison.InvariantCultureIgnoreCase))
                                                                                                                                                                                                        return 0x2E;
                                                                                                                                                                                                    else
                                                                                                                                                                                                        if (sContentType.Equals("text/vnd.wap.sl", StringComparison.InvariantCultureIgnoreCase))
                                                                                                                                                                                                            return 0x2F;
                                                                                                                                                                                                        else
                                                                                                                                                                                                            if (sContentType.Equals("application/vnd.wap.slc", StringComparison.InvariantCultureIgnoreCase))
                                                                                                                                                                                                                return 0x30;
                                                                                                                                                                                                            else
                                                                                                                                                                                                                if (sContentType.Equals("text/vnd.wap.co", StringComparison.InvariantCultureIgnoreCase))
                                                                                                                                                                                                                    return 0x31;
                                                                                                                                                                                                                else
                                                                                                                                                                                                                    if (sContentType.Equals("application/vnd.wap.coc", StringComparison.InvariantCultureIgnoreCase))
                                                                                                                                                                                                                        return 0x32;
                                                                                                                                                                                                                    else
                                                                                                                                                                                                                        if (sContentType.Equals("application/vnd.wap.multipart.related", StringComparison.InvariantCultureIgnoreCase))
                                                                                                                                                                                                                            return 0x33;
                                                                                                                                                                                                                        else
                                                                                                                                                                                                                            if (sContentType.Equals("application/vnd.wap.sia", StringComparison.InvariantCultureIgnoreCase))
                                                                                                                                                                                                                                return 0x34;
                                                                                                                                                                                                                            else
                                                                                                                                                                                                                                if (sContentType.Equals("text/vnd.wap.connectivity-xml", StringComparison.InvariantCultureIgnoreCase))
                                                                                                                                                                                                                                    return 0x35;
                                                                                                                                                                                                                                else
                                                                                                                                                                                                                                    if (sContentType.Equals("application/vnd.wap.connectivity-wbxml", StringComparison.InvariantCultureIgnoreCase))
                                                                                                                                                                                                                                        return 0x36;
                                                                                                                                                                                                                                    else
                                                                                                                                                                                                                                        return 0;
        }

        private int unsignedByte(byte value)
        {
            if (value < 0)
            {
                return (value + 256);
            }
            else
            {
                return value;
            }
        }

        private void WriteValueLength(long value)
        {

            if (value <= 30)
                sw_Out.Write((byte)value);
            else
            {
                sw_Out.Write((byte)31);
                var data = EncodeUintvarNumber(value);
                byte numValue;
                for (int i = 1; i <= data[0]; i++)
                {
                    numValue = data[i];
                    sw_Out.Write(numValue);
                }
            }
        }

        private void WriteUintvar(long value)
        {
            var data = EncodeUintvarNumber(value);
            byte numValue;
            for (int i = 1; i <= data[0]; i++)
            {
                numValue = data[i];
                sw_Out.Write(numValue);
            }
        }

        /**   
         * Encodes the Multimedia Message set by calling setMessage(MMMessage msg)   
         */
        public void EncodeMessage()
        {
            int numValue;
            String strValue;
            m_bMessageEcoded = false;
            m_bMultipartRelated = false;

            if (!m_bMessageAvailable)
                throw new MMEncoderException("No Multimedia Messages set in the encoder");

            try
            {
                m_Out = new MemoryStream();
                sw_Out = new MMBinaryWriter(m_Out, System.Text.Encoding.UTF8);

                if (!m_Message.IsMessageTypeAvailable)
                {
                    sw_Out.Close();
                    throw new MMEncoderException("Invalid Multimedia Message format.");
                }

                byte nMessageType = m_Message.MessageType;

                switch (nMessageType)
                {

                    case MMConstants.MESSAGE_TYPE_M_DELIVERY_IND: // ---------------------------- m-delivery-ind   

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

                    case MMConstants.MESSAGE_TYPE_M_SEND_REQ: // ---------------------------- m-send-req   

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
                            WriteValueLength(strValue.Length + 2);
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
                            WriteValueLength(nCount + 2);

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
                            WriteValueLength(nCount + 2);

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

                                if (!string.IsNullOrWhiteSpace(m_Message.MultipartRelatedType)) {
                                    int valueLength = 1;
                                    String mprt = m_Message.MultipartRelatedType;
                                    valueLength += mprt.Length + 2;
                                    String start = m_Message.PresentationId;
                                    valueLength += start.Length + 2;
                                    // Value-length   
                                    WriteValueLength(valueLength);
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
                                } else {
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
                            bool bRetVal = EncodePart(part);
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

                //sw_Out.Close();   
                m_bMessageEcoded = true;
            }
            catch (IOException e)
            {
                throw new MMEncoderException("An IO error occurred encoding the Multimedia Message.");
            }
        }

        private byte[] EncodeMultiByteNumber(long lData)
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

        private byte[] EncodeUintvarNumber(long lData)
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

        private bool EncodePart(MMContent part)
        {

            if (part == null)
                return false;

            int nHeadersLen = 0; // nHeadersLen = nLengthOfContentType + nLengthOfHeaders   
            int nContentType = 0;

            int nLengthOfHeaders = 0;
            int nLengthOfContentType = 0;


            // -------- HeadersLen = ContentType + Headers fields ---------   
            if ((part.ContentId.Length > 0) && (m_bMultipartRelated))
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
                    WriteUintvar(nHeadersLen);

                    // Write DataLen   
                    WriteUintvar(lDataLen);

                    // Write ContentType   
                    sw_Out.Write((byte)0x03); // length of content type   
                    sw_Out.Write((byte)nContentType);
                    sw_Out.Write((byte)CHARSET_PARAMETER); // charset parameter   
                    sw_Out.Write((byte)0xEA); // us-ascii code   
                }
                else
                {
                    nLengthOfContentType = 1;
                    nHeadersLen = nLengthOfContentType + nLengthOfHeaders;
                    // Write HeadersLen   
                    WriteUintvar(nHeadersLen);
                    // Write DataLen   
                    WriteUintvar(lDataLen);
                    // Write ContentType   
                    sw_Out.Write((byte)nContentType);
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
                    WriteUintvar(nHeadersLen);
                    // Write DataLen   
                    WriteUintvar(lDataLen);

                    // Write ContentType   
                    sw_Out.Write((byte)0x13); //13 characters, actually part.getType().Length+1+1+1   
                    sw_Out.Write(part.Type);
                    //sw_Out.Write(0x00);   
                    sw_Out.Write((byte)FALSE); // charset parameter   
                    sw_Out.Write((byte)0xEA); // ascii-code   
                }
                else
                {
                    nLengthOfContentType = part.Type.Length + 1;
                    // 1 = 0x00   

                    nHeadersLen = nLengthOfContentType + nLengthOfHeaders;

                    // Write HeadersLen   
                    WriteUintvar(nHeadersLen);
                    // Write DataLen   
                    WriteUintvar(lDataLen);
                    // Write ContentType   
                    sw_Out.Write(part.Type);
                    //sw_Out.Write(0x00);   
                }
            }

            if (part.IsContentLocationAvailable)
            {
                // content id   
                sw_Out.Write((byte)0x8E);
                sw_Out.Write(part.ContentLocation);
                //sw_Out.Write(0x00);   
            }

            // Writes the Content ID or the Content Location   
            if ((part.ContentId.Length > 0) && (m_bMultipartRelated))
            {
                sw_Out.Write((byte)0xC0);
                if (part.ContentId.First() == '<')
                {
                    Console.Out.WriteLine("--->QUOTED!!");
                    sw_Out.Write((byte)0x22);
                }
                sw_Out.Write(part.ContentId);
            }

            // ----------- Data --------------   
            byte[] data;
            data = part.GetContent();
            sw_Out.Write(data);

            return true;
        }
    }
}
