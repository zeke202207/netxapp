using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoAddone
{
    public static class ConstEnum
    {
    }

    public enum ExportType
    {
        Floder = 1, //0x1
        Mp4 = 1 << 1, //0x2
        Ts = 1 << 2, //0x4
    }
}
