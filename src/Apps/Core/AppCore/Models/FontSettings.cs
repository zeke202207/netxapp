using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetX.AppCore.Models
{
    public sealed class FontSettings
    {
        public string DefaultFontFamily = "fonts:netxfontfamily#Alibaba PuHuiTi";
        public Uri Key { get; set; } = new Uri("fonts:netxfontfamily", UriKind.Absolute);
        public Uri Source { get; set; } = new Uri("avares://NetX.AppCore/Assets/fonts/alibaba", UriKind.Absolute);
    }
}
