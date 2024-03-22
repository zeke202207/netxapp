using Serilog;
using System;
using System.IO;

namespace NetX.AppCore.Models
{
    public class AppConfig
    {
        public AppInfo Appinfo { get; set; }
    }

    public class AppInfo
    {
        public string Name { get; set; }
        public string Version { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }

        public Stream GetIconStream()
        {
            try
            {
                return File.OpenRead(Icon);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "获取图标失败");
                return default(Stream);
            }
        }
    }

}
