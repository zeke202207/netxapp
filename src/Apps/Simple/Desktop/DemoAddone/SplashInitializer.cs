using LibVLCSharp.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoAddone
{
    public class SplashInitializer
    {
        public static void InitVlcLib()
        {
            Core.Initialize();
        }
    }
}
