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
    public class Encoding_tests
    {
        [Test]
        public void Should_encode_to_expected_data()
        {
            var mms = new MultimediaMessage();
            MultimediaMessageContent content;
            byte[] bytes;

            content = new MultimediaMessageContent();
            bytes = System.IO.File.ReadAllBytes(@".\TestData\megatron.smil");
            content.SetContent(bytes, 0, bytes.Length);
            content.Type = "application/smil";
            content.ContentLocation = "megatron.smil";
            content.ContentId = "<0000>";
            mms.AddContent(content);

            content = new MultimediaMessageContent();
            bytes = System.IO.File.ReadAllBytes(@".\TestData\megatron.txt");
            content.SetContent(bytes, 0, bytes.Length);
            content.Type = "text/plain";
            content.ContentLocation = "megatron.txt";
            content.ContentId = "<megatron.txt>";
            mms.AddContent(content);

            content = new MultimediaMessageContent();
            bytes = System.IO.File.ReadAllBytes(@".\TestData\megatron.png");
            content.SetContent(bytes, 0, bytes.Length);
            content.Type = "image/png";
            content.ContentLocation = "megatron.png";
            content.ContentId = "<megatron.png>";
            mms.AddContent(content);

            content = new MultimediaMessageContent();
            bytes = System.IO.File.ReadAllBytes(@".\TestData\decepticons.png");
            content.SetContent(bytes, 0, bytes.Length);
            content.ContentLocation = "decepticons.png";
            content.Type = "image/png";
            content.ContentId = "<decepticons.png>";

            mms.AddContent(content);

            mms.Date = new DateTime(2015, 3, 26, 9, 27, 31, DateTimeKind.Local);
            mms.TransactionId = "2077.1427358451410";
            mms.ContentType = MultimediaMessageConstants.CT_APPLICATION_MULTIPART_RELATED;
            mms.MessageType = MultimediaMessageConstants.MESSAGE_TYPE_M_SEND_REQ;
            mms.PresentationId = "<0000>";
            mms.DeliveryReport = false;
            mms.Subject = "Test mms";
            mms.IncludeEncodingInSubject = true;
            mms.From = new MultimediaMessageAddress("2077", MultimediaMessageConstants.ADDRESS_TYPE_PLMN);
            mms.AddToAddress("+4798682185/TYPE=PLMN");
            mms.SenderVisibility = MultimediaMessageConstants.SENDER_VISIBILITY_SHOW;
            mms.ReadReply = false;
            mms.MessageClass = MultimediaMessageConstants.MESSAGE_CLASS_PERSONAL;
            mms.Priority = MultimediaMessageConstants.PRIORITY_NORMAL;

            var encoder = new MultimediaMessageEncoder();
           
            encoder.SetMessage(mms);
            encoder.EncodeMessage();
            var messagebytes = encoder.GetMessage();

            var expectedBytes = File.ReadAllBytes(@".\TestData\expected");

            Console.WriteLine(mms.Date.ToString("R"));

            Assert.IsTrue(messagebytes.SequenceEqual(expectedBytes));
        }
    }
}