using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NokiaMMSLibraryNet.Tests
{
    [TestFixture]
    public class Class1
    {
        [Test]
        public void Test()
        {
            var mms = new MMMessage();
            MMContent content;
            byte[] bytes;

            content = new MMContent();
            bytes = System.IO.File.ReadAllBytes(@".\TestData\megatron.smil");
            content.SetContent(bytes, 0, bytes.Length);
            content.Type = "application/smil";
            content.ContentLocation = "megatron.smil";
            content.ContentId = "<0000>";
            mms.AddContent(content);

            content = new MMContent();
            bytes = System.IO.File.ReadAllBytes(@".\TestData\megatron.txt");
            content.SetContent(bytes, 0, bytes.Length);
            content.Type = "text/plain";
            content.ContentLocation = "megatron.txt";
            content.ContentId = "<megatron.txt>";
            mms.AddContent(content);

            content = new MMContent();
            bytes = System.IO.File.ReadAllBytes(@".\TestData\megatron.png");
            content.SetContent(bytes, 0, bytes.Length);
            content.Type = "image/png";
            content.ContentLocation = "megatron.png";
            content.ContentId = "<megatron.png>";
            mms.AddContent(content);

            content = new MMContent();
            bytes = System.IO.File.ReadAllBytes(@".\TestData\decepticons.png");
            content.SetContent(bytes, 0, bytes.Length);
            content.ContentLocation = "decepticons.png";
            content.Type = "image/png";
            content.ContentId = "<decepticons.png>";

            mms.AddContent(content);

            // Mar 26, 2015 09:27:31.000000000 W. Europe Standard Time
            //mms.Date = new DateTime(14273584510000000);
            mms.Date = new DateTime(2015, 3, 26, 9, 27, 31, DateTimeKind.Local);
            Console.WriteLine(mms.Date);
            Console.WriteLine(mms.Date.TotalSeconds());
            mms.TransactionId = "2077.1427358451410";
            mms.ContentType = MMConstants.CT_APPLICATION_MULTIPART_RELATED;
            mms.MessageType = MMConstants.MESSAGE_TYPE_M_SEND_REQ;
            mms.PresentationId = "<0000>";
            mms.DeliveryReport = false;
            mms.Subject = "Test mms";
            mms.IncludeEncodingInSubject = true;
            mms.From = new MMAddress("2077", MMConstants.ADDRESS_TYPE_PLMN);
            mms.AddToAddress("+4798682185/PLMN");
            mms.SenderVisibility = MMConstants.SENDER_VISIBILITY_SHOW;
            mms.ReadReply = false;
            mms.MessageClass = MMConstants.MESSAGE_CLASS_PERSONAL;
            mms.Priority = MMConstants.PRIORITY_NORMAL;

            var encoder = new MMEncoder();
           
            encoder.SetMessage(mms);
            encoder.EncodeMessage();
            var messagebytes = encoder.GetMessage();

            var header = @"POST /mms/no/mt HTTP/1.1
X-NOKIA-MMSC-MESSAGE-TYPE: MultiMediaMessage
X-NOKIA-MMSC-VERSION: 1.0
X-MMSC-TELIA-Version: v2.7
X-MMSC-TELIA-ChargedParty: Recipient
X-MMSC-TELIA-ProductPriceNetCom: 0
Content-Type: application/vnd.wap.mms-message
Content-Length: 52709
Authorization: Basic MjA3NzoycUxMcFQzNm5T
User-Agent: Java1.3.1_03
Host: mmsgw.netcom.no:8080
Accept: text/html, image/gif, image/jpeg, *; q=.2, */*; q=.2
Connection: keep-alive

";

            var stream = File.Create(@"C:\temp\nokiamms_request");

            var headerbytes = System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(header);

            stream.Write(headerbytes, 0, headerbytes.Length);
            stream.Write(messagebytes, 0, messagebytes.Length);
            stream.Close();

            File.WriteAllBytes(@"C:\temp\nokiamms_payload", messagebytes);
        }
    }
}
