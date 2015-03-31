using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NokiaMMSLibraryNet
{
    public static class Extensions
    {
        public static long TotalSeconds(this DateTime time)
        {
            return time.AddYears(-1969).Ticks / 10000000;
        }

        public static int GetMIBEnum(this Encoding encoding)
        {
            switch (encoding.WebName.ToUpper())
            {
                case "US-ASCII": return 3;
                case "ISO-8859-1": return 4;
                case "ISO-8859-2": return 5;
                case "ISO-8859-3": return 6;
                case "ISO-8859-4": return 7;
                case "ISO-8859-5": return 8;
                case "ISO-8859-6": return 9;
                case "ISO-8859-7": return 10;
                case "ISO-8859-8": return 11;
                case "ISO-8859-9": return 12;
                case "SHIFT_JIS": return 17;
                case "EUC-JP": return 18;
                case "ISO-2022-KR": return 37;
                case "EUC-KR": return 38;
                case "ISO-2022-JP": return 39;
                case "UTF-8": return 106;
                case "UTF-7": return 103;
                case "UTF-16": return 1000;
                case "UTF-16BE": return 1013;
                case "UTF-32": return 1017;
                case "UTF-32BE": return 1018;
                case "GB2312": return 2025;
                case "BIG5": return 2026;
                case "WINDOWS-1250": return 2250;
                case "WINDOWS-1251": return 2251;
                case "WINDOWS-1252": return 2252;
                case "WINDOWS-1253": return 2253;
                case "WINDOWS-1254": return 2254;
                case "WINDOWS-1255": return 2255;
                case "WINDOWS-1256": return 2256;
                case "WINDOWS-1257": return 2257;
                case "WINDOWS-1258": return 2258;
                default: return 0;
            }
        }
    }
}
