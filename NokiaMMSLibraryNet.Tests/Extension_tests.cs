using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NokiaMMSLibraryNet;

namespace NokiaMMSLibraryNet.Tests
{
    [TestFixture]
    public class Extension_tests
    {
        [TestCase("ISO-8859-9", 12)]
        [TestCase("Shift_JIS", 17)]
        [TestCase("EUC-JP", 18)]
        [TestCase("ISO-2022-KR", 37)]
        [TestCase("EUC-KR", 38)]
        [TestCase("ISO-2022-JP", 39)]
        [TestCase("UTF-8", 106)]
        [TestCase("UNICODE-1-1-UTF-7", 103)]
        [TestCase("ISO-10646-UCS-2", 1000)]
        [TestCase("UTF-16BE", 1013)]
        [TestCase("UTF-32", 1017)]
        [TestCase("UTF-32BE", 1018)]
        [TestCase("GB2312", 2025)]
        [TestCase("Big5", 2026)]
        [TestCase("windows-1250", 2250)]
        [TestCase("windows-1251", 2251)]
        [TestCase("windows-1252", 2252)]
        [TestCase("windows-1253", 2253)]
        [TestCase("windows-1254", 2254)]
        [TestCase("windows-1255", 2255)]
        [TestCase("windows-1256", 2256)]
        [TestCase("windows-1257", 2257)]
        [TestCase("windows-1258", 2258)]
        
        public void Encoding_to_mibenum(string encodingName, int expectedMib)
        {
            var encoding = System.Text.Encoding.GetEncoding(encodingName);

            Assert.AreEqual(expectedMib, encoding.GetMIBEnum(), "case \"" + encoding.WebName.ToUpper() + "\": return " + expectedMib.ToString() + ";");

        }
    }
}
