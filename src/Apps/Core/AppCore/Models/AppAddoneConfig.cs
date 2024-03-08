using Avalonia.Media;
using Avalonia.Styling;
using Newtonsoft.Json;
using Serilog;
using Splat.ModeDetection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetX.AppCore.Models
{
    public class AppAddoneConfig
    {
        public string[] AddoneAssembly { get; set; }
    }
}
