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

namespace NetX.AppContainer.Models
{
    public class AppConfig
    {
        public Themes Themes { get; set; }
        public Layouts Layouts { get; set; }

        public void Save() => Task.Factory.StartNew(SaveAsync);

        private async Task SaveAsync()
        {
            try
            {
                await File.WriteAllTextAsync(
                     $"{Path.Combine(AppContext.BaseDirectory, AppConst.APP_CONFIG_UI_FILE)}",
                     JsonConvert.SerializeObject(this, new JsonSerializerSettings() { Formatting = Formatting.Indented }));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "配置文件保存失败");
            }
        }
    }

    public class Themes
    {
        public ThemeVariant Theme { get; set; }
        public Themecolor ThemeColor { get; set; }
    }

    public class Themecolor
    {
        public string DisplayName { get; set; }
        public string Primary { get; set; }
        public string Accent { get; set; }
    }

    public class Layouts
    {
        [JsonIgnore]
        public bool AnimationsEnabled { get; set; }
        public bool WindowLocked { get; set; }
        public bool TitlebarVisible { get; set; }
    }
}
