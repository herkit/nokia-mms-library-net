using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NokiaMMSLibraryNet
{
    public class MultimediaMessageDecoder
    {
        private const bool FLAG_DEBUG = false;
        private MultimediaMessage m_Message = null;
        private int m_i = 0;
        private bool m_bMultipartRelated = false;
        private bool m_bMessageAvailable = false;
        private bool m_bHeaderDecoded = false;
        private byte[] m_In;


        // ------------------------------------------------------------- BASIC RULES  
        private long readMultipleByteInt(int length)
        {
            long value = 0L;
            int start = m_i;
            int end = m_i + length - 1;

            for (int ii = end, weight = 1; ii >= start; ii--, weight *= 256)
            {
                int bv = m_In[ii];
                value = value + bv * weight;
            }

            m_i = end + 1;

            return value;
        }

        private String readTextString()
        {
            String value = "";

            if (m_In[m_i] == 0x22)
            {
                // in this case it's a "Quoted-string"  
                m_i++;
            }

            while (m_In[m_i] > 0)
            {
                value = value + (char)m_In[m_i++];
            }

            m_i++;

            return value;
        }

        private Tuple<String, bool> readEncodedStringValue()
        {
            String value = "";
            byte encoding = 0xEA;
            bool encodingSpecified = false;

            // Check if string contains encoding information
            var length = m_In[m_i];
            if (m_In[m_i + length] == 0)
            {
                encoding = m_In[m_i + 1];
                encodingSpecified = true;
                m_i += 2;
            }

            if (m_In[m_i] == 0x22)
            {
                // in this case it's a "Quoted-string"  
                m_i++;
            }

            // TODO: read text using encoding specified above
            while (m_In[m_i] > 0)
            {
                value = value + (char)m_In[m_i++];
            }

            m_i++;

            return new Tuple<string, bool>(value, encodingSpecified);
        }

        private int ReadUintvar()
        {
            int value = 0;
            int bv = m_In[m_i];

            if (bv < 0x80)
            {
                value = bv;
                m_i++;
            }
            else
            { // In this case the format is "Variable Length Unsigned Integer"  
                bool flag = true;
                short count = 0, inc = 0;

                // Count the number of byte needed for the number  
                while (flag)
                {
                    flag = (m_In[m_i + count] & 0x80) == 0x80;
                    count++;
                }

                inc = count;
                count--;

                int weight = 1;
                while (count >= 0)
                {
                    bv = decodeByte(m_In[m_i + count]) * weight;
                    weight *= 128;
                    value = value + bv;
                    count--;
                }

                m_i += inc;
            }

            return value;
        }

        private int readValueLength()
        {
            int length = 0;
            int temp = m_In[m_i++];

            if (temp < 31)
            {
                length = temp;
            }
            else
                if (temp == 31)
                {
                    length = ReadUintvar();
                }

            return length;
        }


        private String readWellKnownMedia()
        {
            String value = "";
            switch (decodeByte(m_In[m_i]))
            {

                case 0x00: value = "*/*"; break;
                case 0x01: value = "text/*"; break;
                case 0x02: value = "text/html"; break;
                case 0x03: value = "text/plain"; break;
                case 0x04: value = "text/x-hdml"; break;
                case 0x05: value = "text/x-ttml"; break;
                case 0x06: value = "text/x-vCalendar"; break;
                case 0x07: value = "text/x-vCard"; break;
                case 0x08: value = "text/vnd.wap.wml"; break;
                case 0x09: value = "text/vnd.wap.wmlscript"; break;
                case 0x0A: value = "text/vnd.wap.channel"; break;
                case 0x0B: value = "multipart/*"; break;
                case 0x0C: value = "multipart/mixed"; break;
                case 0x0D: value = "multipart/form-data"; break;
                case 0x0E: value = "multipart/byteranges"; break;
                case 0x0F: value = "multipart/alternative"; break;
                case 0x10: value = "application/*"; break;
                case 0x11: value = "application/java-vm"; break;
                case 0x12: value = "application/x-www-form-urlencoded"; break;
                case 0x13: value = "application/x-hdmlc"; break;
                case 0x14: value = "application/vnd.wap.wmlc"; break;
                case 0x15: value = "application/vnd.wap.wmlscriptc"; break;
                case 0x16: value = "application/vnd.wap.channelc"; break;
                case 0x17: value = "application/vnd.wap.uaprof"; break;
                case 0x18: value = "application/vnd.wap.wtls-ca-certificate"; break;
                case 0x19: value = "application/vnd.wap.wtls-user-certificate"; break;
                case 0x1A: value = "application/x-x509-ca-cert"; break;
                case 0x1B: value = "application/x-x509-user-cert"; break;
                case 0x1C: value = "image/*"; break;
                case 0x1D: value = "image/gif"; break;
                case 0x1E: value = "image/jpeg"; break;
                case 0x1F: value = "image/tiff"; break;
                case 0x20: value = "image/png"; break;
                case 0x21: value = "image/vnd.wap.wbmp"; break;
                case 0x22: value = "application/vnd.wap.multipart.*"; break;
                case 0x23: value = "application/vnd.wap.multipart.mixed"; break;
                case 0x24: value = "application/vnd.wap.multipart.form-data"; break;
                case 0x25: value = "application/vnd.wap.multipart.byteranges"; break;
                case 0x26: value = "application/vnd.wap.multipart.alternative"; break;
                case 0x27: value = "application/xml"; break;
                case 0x28: value = "text/xml"; break;
                case 0x29: value = "application/vnd.wap.wbxml"; break;
                case 0x2A: value = "application/x-x968-cross-cert"; break;
                case 0x2B: value = "application/x-x968-ca-cert"; break;
                case 0x2C: value = "application/x-x968-user-cert"; break;
                case 0x2D: value = "text/vnd.wap.si"; break;
                case 0x2E: value = "application/vnd.wap.sic"; break;
                case 0x2F: value = "text/vnd.wap.sl"; break;
                case 0x30: value = "application/vnd.wap.slc"; break;
                case 0x31: value = "text/vnd.wap.co"; break;
                case 0x32: value = "application/vnd.wap.coc"; break;
                case 0x33: value = "application/vnd.wap.multipart.related";
                    m_bMultipartRelated = true;
                    break;
                case 0x34: value = "application/vnd.wap.sia"; break;
                case 0x35: value = "text/vnd.wap.connectivity-xml"; break;
                case 0x36: value = "application/vnd.wap.connectivity-wbxml"; break;

            }

            m_i++;

            return value;
        }


        // ------------------------------------------------------- MMS Header Encoding  

        private String readContentTypeValue()
        {
            byte bv = m_In[m_i];
            String value = "";

            if (bv >= 0x80)
            { /* Constrained-media - Short Integer*/
                // Short-integer: the assigned number of the well-known encoding is  
                // small enough to fit into Short-integer  
                value = readWellKnownMedia();
            }
            else /* Constrained-media - Extension-media*/
                if (bv >= 0x20 && bv < 0x80)
                {
                    value = readTextString();
                }
                else /* Content-general-form */
                    if (bv < 0x20)
                    {
                        int valueLength = readValueLength();
                        bv = m_In[m_i];
                        if (bv >= 0x80)
                        { //Well-known-media  
                            int i2 = m_i;
                            value = readWellKnownMedia();
                            if (value.Equals("application/vnd.wap.multipart.related"))
                            {
                                bv = decodeByte(m_In[m_i]);
                                if (bv == MultimediaMessageConstants.WKPA_TYPE)
                                { // Type of the multipart/related  
                                    m_i++;
                                    m_Message.MultipartRelatedType = readTextString();
                                    bv = decodeByte(m_In[m_i]);
                                    if (bv == MultimediaMessageConstants.WKPA_START)
                                    { // Start (it is the pointer to the presentetion part)  
                                        m_i++;
                                        m_Message.PresentationId = readTextString();
                                    }
                                }
                            }

                            m_i = i2 + valueLength;
                        }
                        else
                        {
                            int i2 = m_i;
                            value = readTextString();
                            m_i = i2 + valueLength;
                        }
                    }
            return (value);
        }

        // ------------------------------------------------------------------ MMS Body  
        private void readMMBodyMultiPartRelated()
        {
            int n = 0;
            int c_headerLen = 0, c_dataLen = 0;
            String c_type = "", c_id = "";
            byte[] c_buf;
            int nEntries = m_In[m_i++];

            while (n < nEntries)
            {
                c_headerLen = ReadUintvar();
                c_dataLen = ReadUintvar();
                int freeze_i = m_i;
                c_type = readContentTypeValue();
                int c_typeLen = m_i - freeze_i;

                c_id = "A" + n;
                if (c_headerLen - c_typeLen > 0)
                {
                    if ((decodeByte(m_In[m_i]) == MultimediaMessageConstants.HFN_CONTENT_LOCATION) ||
                     (decodeByte(m_In[m_i]) == MultimediaMessageConstants.HFN_CONTENT_ID))
                    {
                        m_i++;
                        c_id = readTextString();
                    }
                }

                MultimediaMessageContent mmc = new MultimediaMessageContent();
                mmc.Type = c_type;
                mmc.ContentId = c_id;
                mmc.SetContent(m_In, m_i, c_dataLen);
                m_Message.AddContent(mmc);
                m_i += c_dataLen;

                n++;
            }
        }

        private void readMMBodyMultiPartMixed()
        {
            int n = 0;
            int c_headerLen = 0, c_dataLen = 0;
            String c_type = "", c_id = "";
            byte[] c_buf;
            int nEntries = m_In[m_i++];

            while (n < nEntries)
            {
                c_headerLen = ReadUintvar();
                c_dataLen = ReadUintvar();
                c_type = readContentTypeValue();
                c_id = "A" + n;
                if (m_In[m_i] == 0x8E)
                {
                    m_i++;
                    c_id = readTextString();
                }

                if (FLAG_DEBUG) Console.Out.WriteLine("c_type=(" + c_type + ") c_headerLen=(" + c_headerLen + ") c_dataLen=(" + c_dataLen + ") c_id=(" + c_id + ")");

                MultimediaMessageContent mmc = new MultimediaMessageContent();
                mmc.Type = c_type;
                mmc.ContentId = c_id;
                mmc.SetContent(m_In, m_i, c_dataLen);
                m_Message.AddContent(mmc);
                m_i += c_dataLen;


                n++;
            }

        }

        private static DateTime MillisecondsToDate(long msecs)
        {
            return new DateTime(msecs * 10000).AddYears(1969).ToLocalTime();
        }

        // ---------------------------------------------------------------- MMS Header  
        private void readMMHeader()
        {
            bool flag_continue = true;

            while (flag_continue && m_i < m_In.Length)
            {
                byte currentByte = decodeByte(m_In[m_i++]);

                switch (currentByte)
                {
                    case MultimediaMessageConstants.FN_MESSAGE_TYPE:
                        if (FLAG_DEBUG) Console.Out.WriteLine("FN_MESSAGE_TYPE (0C)");
                        m_Message.MessageType = m_In[m_i];
                        m_i++;
                        break;
                    case MultimediaMessageConstants.FN_TRANSACTION_ID:
                        if (FLAG_DEBUG) Console.Out.WriteLine("FN_TRANSACTION_ID (18)");
                        m_Message.TransactionId = readTextString();
                        break;
                    case MultimediaMessageConstants.FN_MESSAGE_ID:
                        if (FLAG_DEBUG) Console.Out.WriteLine("FN_MESSAGE_ID (0B)");
                        m_Message.MessageId = readTextString();
                        break;
                    case MultimediaMessageConstants.FN_STATUS:
                        if (FLAG_DEBUG) Console.Out.WriteLine("FN_STATUS (15)");
                        m_Message.MessageStatus = m_In[m_i];
                        m_i++;
                        break;
                    case MultimediaMessageConstants.FN_MMS_VERSION:
                        if (FLAG_DEBUG) Console.Out.WriteLine("FN_MMS_VERSION (0D)");
                        m_Message.Version = m_In[m_i];
                        m_i++;
                        break;
                    case MultimediaMessageConstants.FN_TO:
                        if (FLAG_DEBUG) Console.Out.WriteLine("FN_TO (17)");
                        m_Message.AddToAddress(readTextString());
                        break;
                    case MultimediaMessageConstants.FN_CC:
                        if (FLAG_DEBUG) Console.Out.WriteLine("FN_CC (02)");
                        m_Message.AddCcAddress(readTextString());
                        break;
                    case MultimediaMessageConstants.FN_BCC:
                        if (FLAG_DEBUG) Console.Out.WriteLine("FN_BCC (01)");
                        m_Message.AddBccAddress(readTextString());
                        break;

                    case MultimediaMessageConstants.FN_DATE:
                        {
                            if (FLAG_DEBUG) Console.Out.WriteLine("FN_DATE (05)");
                            int length = m_In[m_i++];
                            long msecs = readMultipleByteInt(length) * 1000;
                            m_Message.Date = MillisecondsToDate(msecs);
                        }
                        break;

                    case MultimediaMessageConstants.FN_DELIVERY_REPORT:
                        {
                            if (FLAG_DEBUG) Console.Out.WriteLine("FN_DELIVERY_REPORT (06)");
                            int value = m_In[m_i++];
                            if (value == 0x80)
                                m_Message.DeliveryReport = true;
                            else
                                m_Message.DeliveryReport = false;
                            break;
                        }
                    case MultimediaMessageConstants.FN_SENDER_VISIBILITY:
                        if (FLAG_DEBUG) Console.Out.WriteLine("FN_STATUS (14)");
                        m_Message.SenderVisibility = m_In[m_i];
                        m_i++;
                        break;
                    case MultimediaMessageConstants.FN_READ_REPLY:
                        {
                            if (FLAG_DEBUG) Console.Out.WriteLine("FN_READ_REPLY (10)");
                            int value = m_In[m_i++];
                            if (value == 0x80)
                                m_Message.ReadReply = true;
                            else
                                m_Message.ReadReply = false;
                            break;
                        }

                    case MultimediaMessageConstants.FN_FROM:
                        {
                            if (FLAG_DEBUG) Console.Out.WriteLine("FN_FROM (09)");
                            int valueLength = m_In[m_i++];
                            int addressPresentToken = m_In[m_i++];
                            if (addressPresentToken == 0x80)
                            { // Address-present-token  
                                m_Message.From = new MultimediaMessageAddress(readTextString());
                            }
                        }
                        break;
                    case MultimediaMessageConstants.FN_SUBJECT:
                        if (FLAG_DEBUG) Console.Out.WriteLine("FN_SUBJECT (16)");
                        var encodedString = readEncodedStringValue();
                        m_Message.Subject = encodedString.Item1;
                        m_Message.IncludeEncodingInSubject = encodedString.Item2;
                        break;
                    case MultimediaMessageConstants.FN_MESSAGE_CLASS:
                        if (FLAG_DEBUG) Console.Out.WriteLine("FN_MESSAGE_CLASS (0A)");
                        m_Message.MessageClass = m_In[m_i++];
                        break;
                    case MultimediaMessageConstants.FN_EXPIRY:
                        {
                            if (FLAG_DEBUG) Console.Out.WriteLine("FN_EXPIRY (08)");
                            int valueLength = readValueLength();
                            int tokenType = m_In[m_i++];
                            long expiry = 0;

                            if (tokenType == 128)
                            { // Absolute-token  
                                int length = m_In[m_i++];
                                expiry = readMultipleByteInt(length) * 1000;
                                m_Message.IsExpiryAbsolute = true;
                            }

                            if (tokenType == 129)
                            { // Relative-token  
                                m_Message.IsExpiryAbsolute = false;
                                // Read the Delta-seconds-value  
                                if (valueLength > 3)
                                { // Long Integer  
                                    int length = m_In[m_i++];
                                    expiry = readMultipleByteInt(length) * 1000;
                                }
                                else
                                { // Short Integhet (1 OCTECT)  
                                    int b = m_In[m_i] & 0x7F;
                                    expiry = b * 1000;
                                    m_i++;
                                }
                            }
                            m_Message.Expiry = MillisecondsToDate(expiry);
                        }
                        break;
                    case MultimediaMessageConstants.FN_DELIVERY_TIME:
                        {
                            if (FLAG_DEBUG) Console.Out.WriteLine("FN_DELIVERY_TIME (07)");
                            int valueLength = readValueLength();
                            int tokenType = m_In[m_i++];
                            long deliveryTime = 0;

                            if (tokenType == 128)
                            { // Absolute-token  
                                m_Message.IsDeliveryTimeAbsolute = true;
                                int length = m_In[m_i++];
                                deliveryTime = readMultipleByteInt(length) * 1000;
                            }

                            if (tokenType == 129)
                            { // Relative-token  
                                m_Message.IsDeliveryTimeAbsolute = false;
                                // Read the Delta-seconds-value  
                                if (valueLength > 3)
                                { // Long Integer  
                                    int length = m_In[m_i++];
                                    deliveryTime = readMultipleByteInt(length) * 1000;
                                }
                                else
                                { // Short Integhet (1 OCTECT)  
                                    int b = m_In[m_i] & 0x7F;
                                    deliveryTime = b * 1000;
                                    m_i++;
                                }
                            }
                            m_Message.DeliveryTime = MillisecondsToDate(deliveryTime);
                        }
                        break;
                    case MultimediaMessageConstants.FN_PRIORITY:
                        if (FLAG_DEBUG) Console.Out.WriteLine("FN_PRIORITY (0F)");
                        m_Message.Priority = m_In[m_i++];
                        break;
                    case MultimediaMessageConstants.FN_CONTENT_TYPE:
                        if (FLAG_DEBUG) Console.Out.WriteLine("FN_CONTENT_TYPE (04)");
                        m_Message.ContentType = readContentTypeValue();
                        flag_continue = false;
                        break;

                }
            }
        }

        private byte decodeByte(byte value)
        {
            return ((byte)(value & 0x7F));
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
            m_bHeaderDecoded = false;
            m_In = null;
        }

        /** 
         * Sets the buffer representing the Multimedia Message to be decoded. 
         */
        public void setMessage(byte[] buf)
        {
            m_Message = new MultimediaMessage();
            m_In = buf;
            m_bMessageAvailable = true;
        }

        /** 
         * Decodes the header of the Multimedia Message. After calling this method 
         * a MultimediaMessage object, containing just the information related to the header and 
         * without the contents, can be obtained by calling getMessage(). 
         * This method has to be called after setMessage(byte buf[). 
         */
        public void decodeHeader()
        {
            if (m_bMessageAvailable)
            {
                readMMHeader();
                m_bHeaderDecoded = true;
            }
            else
            {
                throw new MultimediaMessageDecoderException("Message unavailable. You must call setMessage(byte[] buf) before calling this method.");
            }
        }

        /** 
         * Decodes the body of the Multimedia Message. This method has to be called 
         * after the method decodeHeader() 
         */
        public void decodeBody()
        {
            if (!m_bHeaderDecoded)
                throw new MultimediaMessageDecoderException("You must call decodeHeader() before calling decodeBody()");

            if ((m_Message.ContentType).CompareTo("application/vnd.wap.multipart.related") == 0)
                readMMBodyMultiPartRelated();
            else
                readMMBodyMultiPartMixed();
        }

        /** 
         * Decodes the whole Multimedia Message. After calling this method 
         * a MultimediaMessage object, containing the information related to the header and 
         * the all contents, can be obtained by calling the method getMessage(). 
         * This method has to be called after setMessage(byte buf[). 
         */
        public void decodeMessage()
        {
            decodeHeader();
            if (m_Message.MessageType == MultimediaMessageConstants.MESSAGE_TYPE_M_SEND_REQ)
                decodeBody();
        }

        public MultimediaMessageDecoder(byte[] buf)
        {
            setMessage(buf);
        }

        public MultimediaMessageDecoder()
        {
        }

        /** 
         * Retrieves the MultimediaMessage object. This method has to be called after the calling 
         * of methods decodeMessage() or decodeHeader(). 
         * 
         * @return An object representing the decoded Multimedia Message 
         */
        public MultimediaMessage getMessage()
        {
            if (m_bHeaderDecoded)
                return m_Message;
            else
                return null;
        }
    }
}
