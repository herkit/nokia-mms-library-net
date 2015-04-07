using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NokiaMMSLibraryNet
{
    public class MultimediaMessage
    {
        /* Internal Flags */
        private bool flagMultipartRelated = false;

        /* Header Fields Flags */
        private bool flagBccAvailable = false;
        private bool flagCcAvailable = false;
        private bool flagContentTypeAvailable = false;
        private bool flagDateAvailable = false;
        private bool flagDeliveryReportAvailable = false;
        private bool flagDeliveryTimeAvailable = false;
        private bool flagDeliveryTimeAbsolute = false;
        private bool flagExpiryAvailable = false;
        private bool flagExpiryAbsolute = false;
        private bool flagFromAvailable = false;
        private bool flagMessageClassAvailable = false;
        private bool flagMessageIdAvailable = false;
        private bool flagMessageTypeAvailable = false;
        private bool flagMMSVersionAvailable = false;
        private bool flagPriorityAvailable = false;
        private bool flagReadReplyAvailable = false;
        private bool flagSenderVisibilityAvailable = false;
        private bool flagStatusAvailable = false;
        private bool flagSubjectAvailable = false;
        private bool flagToAvailable = false;
        private bool flagTransactionIdAvailable = false;


        /* Header Fields */
        private byte hMessageType;
        private string hTransactionId = "";
        private string hMessageId = "";
        private MultimediaMessageVersion hVersion = 0;
        private List<MultimediaMessageAddress> hTo = null;
        private List<MultimediaMessageAddress> hCc = null;
        private List<MultimediaMessageAddress> hBcc = null;
        private DateTime hReceivedDate;
        private MultimediaMessageAddress hFrom = null;
        private string hSubject = "";
        private byte hMessageClass = 0;
        private DateTime hExpiry;
        private bool hDeliveryReport = false;
        private bool hReadReply = false;
        private byte hSenderVisibility = 0;
        private DateTime hDeliveryTime;
        private byte hPriority;
        private string hContentType = "";
        private MultimediaMessageStatus hStatus;


        /* Others variables */
        private string multipartRelatedType = "";
        private string presentationId = "";
        private Dictionary<string, MultimediaMessageContent> tableOfContents = null;


        /** 
         * Check if the presentation part is available. 
         * 
         * return true if availale. 
         */
        public bool IsPresentationAvailable
        {
            get
            {
                return flagMultipartRelated;
            }
        }

        /** 
         * Check if the message type field is available. 
         * 
         * return true if availale. 
         */
        public bool IsMessageTypeAvailable
        {
            get
            {
                return flagMessageTypeAvailable;
            }
        }

        /** 
         * Check if the Delivery-Report field is available. 
         * 
         * return true if availale. 
         */
        public bool IsDeliveryReportAvailable
        {
            get {
                return flagDeliveryReportAvailable;
            }
        }

        /** 
         * Check if the Sender-Visibility field is available. 
         * 
         * return true if availale. 
         */
        public bool IsSenderVisibilityAvailable
        {
            get
            {
                return flagSenderVisibilityAvailable;
            }
        }


        /** 
         * Check if the Read-Reply field is available. 
         * 
         * return true if availale. 
         */
        public bool IsReadReplyAvailable
        {
            get
            {
                return flagReadReplyAvailable;
            }
        }


        /** 
         * Check if the Status field is available. 
         * 
         * return true if availale. 
         */
        public bool IsStatusAvailable
        {
            get
            {
                return flagStatusAvailable;
            }
        }

        /** 
         * Check if the transaction ID field is available. 
         * 
         * return true if availale. 
         */
        public bool IsTransactionIdAvailable
        {
            get
            {
                return flagTransactionIdAvailable;
            }
        }

        /** 
         * Check if the message ID field is available. 
         * 
         * return true if availale. 
         */
        public bool IsMessageIdAvailable
        {
            get
            {
                return flagMessageIdAvailable;
            }
        }

        /** 
         * Check if the version field is available. 
         * 
         * return true if availale. 
         */
        public bool IsVersionAvailable
        {
            get
            {
                return flagMMSVersionAvailable;
            }
        }

        /** 
         * Check if the date field is available. 
         * 
         * return true if availale. 
         */
        public bool IsDateAvailable
        {
            get
            {
                return flagDateAvailable;
            }
        }

        /** 
         * Check if sender address field is available. 
         * 
         * return true if availale. 
         */
        public bool IsFromAvailable
        {
            get
            {
                return flagFromAvailable;
            }
        }

        /** 
         * Check if the subject field is available. 
         * 
         * return true if availale. 
         */
        public bool IsSubjectAvailable
        {
            get
            {
                return flagSubjectAvailable;
            }
        }

        public bool IncludeEncodingInSubject { get; set; }

        /** 
         * Check if the message class field is available. 
         * 
         * return true if availale. 
         */
        public bool IsMessageClassAvailable
        {
            get
            {
                return flagMessageClassAvailable;
            }
        }

        /** 
         * Check if the expiry date/time field is available. 
         * 
         * return true if availale. 
         */
        public bool IsExpiryAvailable
        {
            get
            {
                return flagExpiryAvailable;
            }
        }

        /** 
         * Check if the delivery date/time field is available. 
         * 
         * return true if availale. 
         */
        public bool IsDeliveryTimeAvailable
        {
            get
            {
                return flagDeliveryTimeAvailable;
            }
        }

        /** 
         * Check if the priority date/time field is available. 
         * 
         * return true if availale. 
         */
        public bool IsPriorityAvailable
        {
            get
            {
                return flagPriorityAvailable;
            }
        }

        /** 
         * Check if the content type field is available. 
         * 
         * return true if availale. 
         */
        public bool IsContentTypeAvailable
        {
            get
            {
                return flagContentTypeAvailable;
            }
        }

        /** 
         * Check if there is at least one receiver is specified. 
         * 
         * return true if at least one receiver is specified. 
         */
        public bool IsToAvailable
        {
            get
            {
                return flagToAvailable;
            }
        }

        /** 
         * Check if there is at least one BCC receiver is specified. 
         * 
         * return true if at least one BCC receiver is specified. 
         */
        public bool IsBccAvailable
        {
            get
            {
                return flagBccAvailable;
            }
        }

        /** 
         * Check if there is at least one CC receiver is specified. 
         * 
         * return true if at least one CC receiver is specified. 
         */
        public bool IsCcAvailable
        {
            get
            {
                return flagCcAvailable;
            }
        }

        /** 
         * Returns the type of the message (Mandatory). 
         * 
         * return the type of the message. 
         * IMMConstants.MESSAGE_TYPE_M_SEND_REQ. 
         * IMMConstants.MESSAGE_TYPE_M_DELIVERY_IND. 
         * 
         * Sets the type of the message (Mandatory). 
         * Specifies the transaction type. 
         * 
         * param value the type of the message. 
         * IMMConstants.MESSAGE_TYPE_M_SEND_REQ. 
         * IMMConstants.MESSAGE_TYPE_M_DELIVERY_IND. 
         */
        public byte MessageType
        {
            get
            {
                return hMessageType;
            }
            set
            {
                hMessageType = value;
                flagMessageTypeAvailable = true;
            }
        }

        /** 
         * Retrieves the Message ID (Mandatory). 
         * Identifier of the message. From Send request, connects delivery report to sent message in MS. 
         * 
         * return the message ID. 
         * 
         * Sets the message ID (Mandatory). 
         * Identifier of the message. From Send request, connects delivery report to sent message in MS. 
         * 
         * param value the message ID. 
         */
        public string MessageId
        {
            get
            {
                return hMessageId;
            }
            set
            {
                hMessageId = value;
                flagMessageIdAvailable = true;
            }
        }

        /** 
         * Retrieves the Message Status (Mandatory). 
         * The status of the message. 
         * 
         * return the message Status. 
         * 
         * Sets the message Status (Mandatory). 
         * The status of the message. 
         * 
         * param value the message Status. 
         */
        public MultimediaMessageStatus MessageStatus
        {
            get
            {
                return hStatus;
            }
            set
            {
                hStatus = value;
                flagStatusAvailable = true;
            }
        }

        /** 
         * Retrieves the transaction ID (Mandatory). 
         * It is a unique identifier for the message and it identifies the M-Send.req 
         * and the corresponding reply only. 
         * 
         * return the transaction ID. 
         * 
         * Sets the transaction ID (Mandatory). 
         * It is a unique identifier for the message and it identifies the M-Send.req 
         * and the corresponding reply only. 
         * 
         * param value the trensaction ID. 
         */
        public string TransactionId
        {
            get
            {
                return hTransactionId;
            }
            set
            {
                hTransactionId = value;
                flagTransactionIdAvailable = true;
            }
        }

        /** 
         * Retrieves the MMS version number as a string(Mandatory). 
         * 
         * return the version as a string. The only supported value are "1.0" and "1.1". 
         */
        public string GetVersionAsString()
        {
            int ver_major = ((byte)((byte)hVersion << 1)) >> 5;
            int ver_minor = ((byte)((byte)hVersion << 4)) >> 4;
            string result = ver_major + "." + ver_minor;

            return result;
        }

        /** 
         * Retrieves the MMS version number (Mandatory). 
         * 
         * return the version. The only supported value are  
         * IMMConstants.MMS_VERSION_10 and IMMConstants.MMS_VERSION_11 
         * 
         * Sets the MMS version number (Mandatory). 
         * 
         * param value the only supported value are  
         * IMMConstants.MMS_VERSION_10 and IMMConstants.MMS_VERSION_11 
         */
        public MultimediaMessageVersion Version
        {
            get
            {
                return hVersion;
            }
            set
            {
                hVersion = value;
                flagMMSVersionAvailable = true;
            }
        }

        /** 
         * Adds a new receiver of the Multimedia Message. The message can have 
         * more than one receiver but at least one. 
         * 
         * param value is the string representing the address of the receiver. It has 
         * to be specified in the full format i.e.: +358990000005/TYPE=PLMN or 
         * joe@user.org or 1.2.3.4/TYPE=IPv4.  
         * 
         */
        public void AddToAddress(string value)
        {
            MultimediaMessageAddress addr = new MultimediaMessageAddress(value);
            hTo.Add(addr);
            flagToAvailable = true;
        }

        /** 
         * Adds a new receiver in the CC (Carbon Copy) field of the Multimedia Message. The message can have 
         * more than one receiver in the CC field. 
         * 
         * param value is the string representing the address of the CC receiver. It has 
         * to be specified in the full format i.e.: +358990000005/TYPE=PLMN or 
         * joe@user.org or 1.2.3.4/TYPE=IPv4.  
         * 
         */
        public void AddCcAddress(string value)
        {
            MultimediaMessageAddress addr = new MultimediaMessageAddress(value);
            hCc.Add(addr);
            flagCcAvailable = true;
        }

        /** 
         * Adds a new receiver in the BCC (Blind Carbon Copy) field of the Multimedia Message. The message can have 
         * more than one receiver in the BCC field. 
         * 
         * param value is the string representing the address of the BCC receiver. It has 
         * to be specified in the full format i.e.: +358990000005/TYPE=PLMN or 
         * joe@user.org or 1.2.3.4/TYPE=IPv4. 
         * 
         */
        public void AddBccAddress(string value)
        {
            MultimediaMessageAddress addr = new MultimediaMessageAddress(value);
            hBcc.Add(addr);
            flagBccAvailable = true;
        }

        /** 
         * Clears all the receivers of the Multimedia Message. 
         * 
         */
        public void ClearTo()
        {
            hTo.Clear();
            flagToAvailable = false;
        }

        /** 
         * Clears all the carbon copy receivers of the Multimedia Message. 
         * 
         */
        public void ClearCc()
        {
            hCc.Clear();
            flagCcAvailable = false;
        }

        /** 
         * Clears all the blind carbon copy receivers of the Multimedia Message. 
         * 
         */
        public void ClearBcc()
        {
            hBcc.Clear();
            flagBccAvailable = false;
        }

        /** 
         * Retrieve all the receivers of the Multimedia Message. 
         * 
         * return a vector of MMAddress objects. 
         * 
         */
        public List<MultimediaMessageAddress> To
        {
            get
            {
                return hTo;
            }
        }

        /** 
         * Retrieve all the CC receivers of the Multimedia Message. 
         * 
         * return a vector of MMAddress objects. 
         * 
         */
        public List<MultimediaMessageAddress> Cc
        {
            get
            {
                return hCc;
            }
        }

        /** 
         * Retrieve all the BCC receivers of the Multimedia Message. 
         * 
         * return a vector of MMAddress objects. 
         * 
         */
        public List<MultimediaMessageAddress> Bcc
        {
            get
            {
                return hBcc;
            }
        }

        /** 
         * Retrieves the arrival time of the message at the MMS Proxy-Relay (Optional). 
         * MMS Proxy-Relay will generate this field when not supplied by terminal. 
         * 
         * return the arrival date. 
         * 
         * Sets the sending time of the message at the MMS Proxy-Relay (Optional). 
         * 
         * param value the date. 
         * 
         */
        public DateTime Date
        {
            get
            {
                return hReceivedDate;
            }
            set
            {
                hReceivedDate = value;
                flagDateAvailable = true;
            }
        }


        /** 
         * Retrieves the address of the message sender (Mandatory). 
         * 
         * return the address. 
         * 
         * Sets the address of the message sender (Mandatory). 
         * 
         * param value is the string representing the address of the sender. It has 
         * to be specified in the full format i.e.: +358990000005/TYPE=PLMN or 
         * joe@user.org or 1.2.3.4/TYPE=IPv4. 
         * 
         */
        public MultimediaMessageAddress From
        {
            get
            {
                return hFrom;
            }
            set
            {
                hFrom = value;
                flagFromAvailable = true;
            }
        }


        /** 
         * Retrieves the subject of the Multimedia Message (Optional). 
         * 
         * return the subject. 
         * 
         * Sets the subject of the Multimedia Message (Optional). 
         * 
         * param value the subject. 
         */
        public string Subject
        {
            get
            {
                return hSubject;
            }
            set
            {
                hSubject = value;
                flagSubjectAvailable = true;
            }
        }


        /** 
         * Retrieves the message class of the Multimedia Message (Optional). 
         * 
         * return One of the following values: 
         * MESSAGE_CLASS_PERSONAL, MESSAGE_CLASS_ADVERTISEMENT, 
         * MESSAGE_CLASS_INFORMATIONAL, MESSAGE_CLASS_AUTO 
         * 
         * Sets the message class of the Multimedia Message (Optional). 
         * 
         * param value is the message class. It can have one of the following values: 
         * MESSAGE_CLASS_PERSONAL, MESSAGE_CLASS_ADVERTISEMENT, 
         * MESSAGE_CLASS_INFORMATIONAL, MESSAGE_CLASS_AUTO 
         *
         */
        public byte MessageClass
        {
            get
            {
                return hMessageClass;
            }
            set
            {
                hMessageClass = value;
                flagMessageClassAvailable = true;
            }
        }

        /** 
         * Retrieves the content type of the Multimedia Message (Mandatory). 
         * 
         * return the content type. 
         * 
         * Sets the content type of the Multimedia Message (Mandatory). 
         * 
         * param value is the content type. The standard for interoperability supports 
         * one of the following values: 
         * CT_APPLICATION_MULTIPART_MIXED, CT_APPLICATION_MULTIPART_RELATED 
         * 
         */
        public string ContentType
        {
            get
            {
                return hContentType;
            }
            set
            {
                hContentType = value;
                flagContentTypeAvailable = true;
            }
        }

        /** 
         * Retrieves the number of contents of of the Multimedia Message (Mandatory). 
         * 
         * return the number of contents. 
         */
        public int NumContents
        {
            get
            {
                return tableOfContents.Count;
            }
        }

        /** 
         * Sets the type of the presentation part.(Mandatory when the content type of the Multimedia Message is CT_APPLICATION_MULTIPART_RELATED). 
         * 
         * param value the type of the presentation part. The standard for interoperability supports 
         * only the  value: CT_APPLICATION_SMIL 
         * 
         * Retrieves the type of the presentation part. 
         * 
         * return the type of the presentation part. 
         */
        public string MultipartRelatedType
        {
            set
            {
                flagMultipartRelated = true;
                multipartRelatedType = value;
            }
            get
            {
                if (flagMultipartRelated)
                {
                    return multipartRelatedType;
                }
                else
                    return null;
            }
        }

        /** 
         * Adds a content to the message. 
         * 
         * param mmc is the content to add. 
         * 
         */
        public void AddContent(MultimediaMessageContent mmc)
        {
            tableOfContents.Add(mmc.ContentId, mmc);
        }

        /** 
         * Retrieves the content referring to the presentation part. 
         * 
         * return the presentation content. 
         */
        public MultimediaMessageContent Presentation
        {
            get
            {
                if ((flagMultipartRelated == true) && (NumContents > 0) && tableOfContents.ContainsKey(presentationId))
                    return (MultimediaMessageContent)tableOfContents[presentationId];
                else
                    return null;
            }
        }

        /** 
         * Retrieves the content ID referring to the presentation part. 
         * 
         * return the content ID. 
         * 
         * Sets the content ID of the content containing the presentation part of the 
         * Multimedia Message (Mandatory when the content type of the Multimedia Message is CT_APPLICATION_MULTIPART_RELATED). 
         * 
         * param value is the content ID. 
         */
        public string PresentationId
        {
            get
            {
                if ((flagMultipartRelated == true) && (NumContents > 0) && tableOfContents.ContainsKey(presentationId))
                    return presentationId;
                else
                    return null;
            }
            set
            {
                presentationId = value;
            }
        }

        /** 
         * Retrieves the content having the specified id. 
         * 
         * return the content. 
         * param id is the id of the content to be retrieved. 
         */
        public MultimediaMessageContent GetContent(string id)
        {
            return (MultimediaMessageContent)tableOfContents[id];
        }

        /** 
         * Retrieves the content at the position index. 
         * 
         * return the content. 
         * param index is the index of the content to be retrieved. 
         */
        public MultimediaMessageContent GetContent(int index)
        {
            return tableOfContents.Values.ToArray()[index];
        }

        /** 
         * Retrieves the expiry date of the message (Optional). 
         * 
         * return the expiry date/time. 
         * 
         * Sets the length of time the message will be stored in the MMS Proxy-Relay 
         * or time to delete the message (Optional). This field can have two format, 
         * either absolute or interval depending on how it is set with the method 
         * setExpiryAbsolute(). 
         * 
         * param value is the expiry date/time. 
         */
        public DateTime Expiry
        {
            get
            {
                return hExpiry;
            }
            set
            {
                hExpiry = value;
                flagExpiryAvailable = true;
            }
        }

        /** 
         * Sets the format of the expiry date/time. 
         * 
         * param value if true the expiry date is absolute, else is 
         * intended as an interval. 
         * 
         * Returns information about the format of the expiry date/time. 
         * 
         * return true if the expiry date/time is absolute, 
         * false if it is intended as an interval. 
         */
        public bool IsExpiryAbsolute
        {
            get
            {
                return flagExpiryAbsolute;
            }
            set
            {
                flagExpiryAbsolute = value;
            }
        }

        /** 
         * Retrieves the delivery-report flag (Optional). 
         * 
         * return  true if the user wants the delivery report. 
         * 
         * Specify whether the user wants a delivery report from each recipient (Optional). 
         * 
         * param value true if the user wants the delivery report. 
         * 
         */
        public bool DeliveryReport
        {
            get
            {
                return hDeliveryReport;
            }
            set
            {
                hDeliveryReport = value;
                flagDeliveryReportAvailable = true;
            }
        }

        /** 
         * Retrieves the sender-visibility flag (Optional). 
         * 
         * return  0x80 if the user wants the sender visibility setting to Hide. 
         * 0x81 if the user wants the sender visibility setting to Show. 
         * 
         * Specify whether the user wants sender visibility (Optional). 
         * 
         * return  0x80 if the user wants the sender visibility setting to Hide. 
         * 0x81 if the user wants the sender visibility setting to Show. 
         * 
         */
        public byte SenderVisibility
        {
            get
            {
                return hSenderVisibility;
            }
            set
            {
                hSenderVisibility = value;
                flagSenderVisibilityAvailable = true;
            }
        }

        /** 
         * Retrieves the read reply flag (Optional). 
         * 
         * return  true if the user wants the read reply. 
         * 
         * Specify whether the user wants a read report from each recipient as a new message(Optional). 
         * 
         * param value true if the user wants the read report. 
         */
        public bool ReadReply
        {
            get
            {
                return hReadReply;
            }
            set
            {
                hReadReply = value;
                flagReadReplyAvailable = true;
            }
        }

        /** 
         * Retrieves the delivery date/time of the message (Optional). 
         * 
         * return the delivery date/time. 
         * 
         * Sets the date/time of the desired delivery(Optional). 
         * Indicates the earliest possible delivery of the message to the recipient. 
         * This field can have two format, 
         * either absolute or interval depending on how it is set with the method 
         * setDeliveryTimeAbsolute(). 
         * 
         * param value the delivery date/time. 
         */
        public DateTime DeliveryTime
        {
            get
            {
                return hDeliveryTime;
            }
            set
            {
                hDeliveryTime = value;
                flagDeliveryTimeAvailable = true;
            }
        }

        /** 
         * Returns information about the format of the delivery date/time. 
         * 
         * return true if the delivery date/time is absolute, 
         * false if it is intended as an interval. 
         * 
         * Sets the format of the delivery date/time. 
         * 
         * param value if true the delivery date/time is absolute, else is 
         * intended as an interval. 
         */
        public bool IsDeliveryTimeAbsolute
        {
            get
            {
                return flagDeliveryTimeAbsolute;
            }
            set
            {
                flagDeliveryTimeAbsolute = value;
            }
        }

        /** 
         * Retrieves the priority of the message for the recipient (Optional). 
         * 
         * return the priority. It can be one of the following the values: 
         * PRIORITY_LOW, PRIORITY_NORMAL, PRIORITY_HIGH 
         * 
         * Sets the priority of the message for the recipient (Optional). 
         * 
         * param value One of the following the values: 
         * PRIORITY_LOW, PRIORITY_NORMAL, PRIORITY_HIGH 
         */
        public byte Priority
        {
            get
            {
                return hPriority;
            }
            set
            {
                hPriority = value;
                flagPriorityAvailable = true;
            }
        }

        public MultimediaMessage()
        {
            tableOfContents = new Dictionary<string, MultimediaMessageContent>();
            hTo = new List<MultimediaMessageAddress>();
            hCc = new List<MultimediaMessageAddress>();
            hBcc = new List<MultimediaMessageAddress>();
        }  
    }
}
