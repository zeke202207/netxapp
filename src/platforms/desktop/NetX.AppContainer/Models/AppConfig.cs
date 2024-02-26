using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetX.AppContainer.Models
{
    public class AppConfig
    {
        public Windows windows { get; set; }
        public Splashscreen splashscreen { get; set; }
    }

    public class Windows
    {
        public string icon { get; set; }
    }

    public class Splashscreen
    {
        public int width { get; set; }
        public int height { get; set; }
        public string image { get; set; }
        public string fontcolor { get;set; }
    }

}
