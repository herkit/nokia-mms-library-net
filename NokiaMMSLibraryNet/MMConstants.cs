using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NokiaMMSLibraryNet
{
    public class MMConstants
    {
        public const byte MMS_VERSION_10 = (byte)0x90; // V1.0  
  
        /* Field Names Costants */  
        public const byte FN_BCC=0x01;  
        public const byte FN_CC=0x02;  
        public const byte FN_CONTENT_TYPE=0x04;  
        public const byte FN_DATE=0x05;  
        public const byte FN_DELIVERY_REPORT=0x06;  
        public const byte FN_DELIVERY_TIME=0x07;  
        public const byte FN_EXPIRY=0x08;  
        public const byte FN_FROM=0x09;  
        public const byte FN_MESSAGE_CLASS=0x0A;  
        public const byte FN_MESSAGE_ID=0x0B;  
        public const byte FN_MESSAGE_TYPE=0x0C;  
        public const byte FN_MMS_VERSION=0x0D;  
        public const byte FN_PRIORITY=0x0F;  
        public const byte FN_READ_REPLY=0x10;  
        public const byte FN_SENDER_VISIBILITY=0x14;  
        public const byte FN_STATUS=0x15;  
        public const byte FN_SUBJECT=0x16;  
        public const byte FN_TO=0x17;  
        public const byte FN_TRANSACTION_ID=0x18;  


        public const byte ADDRESS_TYPE_UNKNOWN=0;  
        public const byte ADDRESS_TYPE_PLMN=1;  
        public const byte ADDRESS_TYPE_IPV4=2;  
        public const byte ADDRESS_TYPE_IPV6=3;  
        public const byte ADDRESS_TYPE_EMAIL=4;  
  
        public const byte MESSAGE_TYPE_M_SEND_REQ=(byte)0x80;  
        public const byte MESSAGE_TYPE_M_DELIVERY_IND=(byte)0x86;  
  
        public const byte MESSAGE_STATUS_EXPIRED =(byte)0x80;  
        public const byte MESSAGE_STATUS_RETRIEVED =(byte)0x81;  
        public const byte MESSAGE_STATUS_REJECTED =(byte)0x82;  
        public const byte MESSAGE_STATUS_DEFERRED =(byte)0x83;  
        public const byte MESSAGE_STATUS_UNRECOGNISED =(byte)0x84;  
  
        public const byte SENDER_VISIBILITY_HIDE =(byte)0x80;  
        public const byte SENDER_VISIBILITY_SHOW =(byte)0x81;  
  
        public const byte MESSAGE_CLASS_PERSONAL =(byte)0x80;  
        public const byte MESSAGE_CLASS_ADVERTISEMENT =(byte)0x81;  
        public const byte MESSAGE_CLASS_INFORMATIONAL = (byte)0x82;  
        public const byte MESSAGE_CLASS_AUTO = (byte)0x83;  
  
        public const byte PRIORITY_LOW = (byte) 0x80;  
        public const byte PRIORITY_NORMAL = (byte) 0x81;  
        public const byte PRIORITY_HIGH = (byte) 0x82;  
  
  
        /* Header Field Name of the Content (Entry)*/  
        public const byte HFN_CONTENT_LOCATION=0x0E;  
        public const byte HFN_CONTENT_ID=0x40;  
  
        /* Well-known Parameter Assignments */  
        public const byte WKPA_TYPE=0x09;  
        public const byte WKPA_START=0x0A;  
  
        /* Content Types strings*/  
        public const string CT_TEXT_HTML="text/html";  
        public const string CT_TEXT_PLAIN="text/plain";  
        public const string CT_TEXT_WML="text/vnd.wap.wml";  
        public const string CT_IMAGE_GIF="image/gif";  
        public const string CT_IMAGE_JPEG="image/jpeg";  
        public const string CT_IMAGE_TIFF="image/tiff";  
        public const string CT_IMAGE_PNG="image/png";  
        public const string CT_IMAGE_WBMP="image/vnd.wap.wbmp";  
        public const string CT_APPLICATION_MULTIPART_MIXED="application/vnd.wap.multipart.mixed";  
        public const string CT_APPLICATION_MULTIPART_RELATED="application/vnd.wap.multipart.related";  
        public const string CT_APPLICATION_SMIL="application/smil";  
    }
}
