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

        [Test]
        public void Should_encode_delivery_indication()
        {
            var msg = new MultimediaMessage();

            msg.MessageType = MultimediaMessageConstants.MESSAGE_TYPE_M_DELIVERY_IND;
            msg.MessageId = "1234";
            msg.Version = MultimediaMessageVersion.Version10;
            msg.Date = new DateTime(2015, 3, 26, 9, 27, 31, DateTimeKind.Local);
            msg.MessageStatus = MultimediaMessageStatus.RETRIEVED;

            msg.AddToAddress("+4790871951/TYPE=PLMN");

            var ms = new MemoryStream();
            var encoder = new MultimediaMessageEncoder();

            encoder.EncodeMessage(msg, ms);

            var bytes = ms.ToArray();

            Assert.AreEqual(0x8C, bytes[0]);
            Assert.AreEqual(0x86, bytes[1]);

            // Message id
            Assert.AreEqual(0x8B, bytes[2]);
            Assert.AreEqual(System.Text.Encoding.UTF8.GetBytes("1234"), bytes.Skip(3).Take(4).ToArray());
            Assert.AreEqual(0x00, bytes[7]);

            Assert.AreEqual(0x8D, bytes[8]);
            Assert.AreEqual(0x90, bytes[9]);

            Assert.AreEqual(0x85, bytes[10]);
            Assert.AreEqual(new[] { 0x04, 0x55, 0x13, 0xC2, 0xF3 }, bytes.Skip(11).Take(5), "Date is not given properly");

            Assert.AreEqual(0x97, bytes[16]);
            Assert.AreEqual(System.Text.Encoding.UTF8.GetBytes("+4790871951/TYPE=PLMN"), bytes.Skip(17).Take(21).ToArray());
            Assert.AreEqual(0x00, bytes[38]);

            Assert.AreEqual(0x95, bytes[39]);
            Assert.AreEqual(0x81, bytes[40]);

            Assert.AreEqual(41, bytes.Length);
        }
    }
}