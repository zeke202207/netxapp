using Avalonia.Media;
using Avalonia.Styling;
using Newtonsoft.Json;
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

        public void Save()
        {
            File.WriteAllText(
                $"{Path.Combine(AppContext.BaseDirectory, "appsetting.ui.json")}", 
                JsonConvert.SerializeObject(this,new JsonSerializerSettings() { Formatting = Formatting.Indented}));
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
