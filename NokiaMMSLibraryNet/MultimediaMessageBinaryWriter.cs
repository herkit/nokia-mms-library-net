using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NokiaMMSLibraryNet
{
    public class MultimediaMessageBinaryWriter : BinaryWriter
    {
        private readonly Encoding _encoding;
        public MultimediaMessageBinaryWriter(Stream output)
            : base(output)
        {
            _encoding = System.Text.Encoding.Default;
        }

        public MultimediaMessageBinaryWriter(Stream output, Encoding encoding)
            : base(output, encoding)
        {
            _encoding = encoding;
        }

        public MultimediaMessageBinaryWriter(Stream output, Encoding encoding, bool leaveOpen)
            : base(output, encoding, leaveOpen)
        {
            _encoding = encoding;
        }

        public Encoding Encoding { get { return _encoding; } }

        public override void Write(string value)
        {
            base.Write(_encoding.GetBytes(value));
            base.Write((byte)0x00);
        }

        public void WriteEncodedString(string value)
        {
            var bytes = _encoding.GetBytes(value);
            var mib = _encoding.GetMIBEnum() + 0x80;
            base.Write((byte)((bytes.Length + 2) % 256));
            base.Write((byte)(mib));
            base.Write(bytes);
            base.Write((byte)0x00);
        }
    }
}
