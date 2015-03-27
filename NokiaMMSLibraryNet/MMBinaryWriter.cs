using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NokiaMMSLibraryNet
{
    public class MMBinaryWriter : BinaryWriter
    {
        private readonly Encoding _encoding;
        public MMBinaryWriter(Stream output)
            : base(output)
        {
            _encoding = System.Text.Encoding.Default;
        }

        public MMBinaryWriter(Stream output, Encoding encoding)
            : base(output, encoding)
        {
            _encoding = encoding;
        }

        public MMBinaryWriter(Stream output, Encoding encoding, bool leaveOpen)
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
    }
}
