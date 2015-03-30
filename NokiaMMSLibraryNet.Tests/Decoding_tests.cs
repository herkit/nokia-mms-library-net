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
    public class Decoding_tests
    {
        [Test]
        public void Deserializes_data_correctly()
        {
            var decoder = new MultimediaMessageDecoder();
            decoder.setMessage(File.ReadAllBytes(@".\TestData\expected"));

            decoder.decodeMessage();

            var message = decoder.getMessage();

            Console.WriteLine(string.Join(Environment.NewLine, message.Date.ToString("R")));

            Assert.AreEqual("2077.1427358451410", message.TransactionId);
            Assert.AreEqual(MultimediaMessageConstants.CT_APPLICATION_MULTIPART_RELATED, message.ContentType);
            Assert.AreEqual(MultimediaMessageConstants.MESSAGE_TYPE_M_SEND_REQ, message.MessageType);
            Assert.AreEqual(new DateTime(2015, 3, 26, 9, 27, 31), message.Date);
            Assert.AreEqual(false, message.DeliveryReport);
            Assert.AreEqual("+4798682185", message.To.First().Address);
            Assert.AreEqual(MultimediaMessageConstants.ADDRESS_TYPE_PLMN, message.To.First().Type);
            Assert.AreEqual("2077", message.From.Address);
            Assert.AreEqual(MultimediaMessageConstants.ADDRESS_TYPE_PLMN, message.From.Type);
            Assert.AreEqual("Test mms", message.Subject);
            Assert.AreEqual(true, message.IncludeEncodingInSubject);
            Assert.AreEqual(MultimediaMessageConstants.SENDER_VISIBILITY_SHOW, message.SenderVisibility);
            Assert.AreEqual(MultimediaMessageConstants.MESSAGE_CLASS_PERSONAL, message.MessageClass);
            Assert.AreEqual(MultimediaMessageConstants.PRIORITY_NORMAL, message.Priority);
            Assert.AreEqual(false, message.ReadReply);

            Assert.AreEqual(4, message.NumContents);
        }
    }
}
