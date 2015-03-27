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
    }
}
