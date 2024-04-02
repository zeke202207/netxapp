using Avalonia.Styling;
using Newtonsoft.Json;
using Serilog;
using System;
using System.IO;
using System.Threading.Tasks;

namespace NetX.AppCore.Models
{
    public class AppUserConfig
    {
        public Themes Themes { get; set; }
        public Layouts Layouts { get; set; }

        public void Save() => Task.Factory.StartNew(SaveAsync);

        private async Task SaveAsync()
        {
            try
            {
                await File.WriteAllTextAsync(
                     $"{Path.Combine(AppContext.BaseDirectory, AppConst.APP_CONFIG_USER_FILE)}",
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
        public string Theme { get; set; }
        public bool IsCustomAccent { get; set; }
        public string AccentColor { get; set; }
    }

    public class Layouts
    {
        public Navigationview Navigationview { get; set; }
    }

    public class Navigationview
    {
        [JsonIgnore]
        public Action<string,object> PropertyHasChanged;

        public string PaneDisplayMode { get; set; }
        public bool CanToggle { get; set; }
        public bool IsSingleContentPage { get; set; }
        public int OpenPaneLength { get; set; }

        public void OnPropertyChange(string propertyName)
        {
            PropertyHasChanged?.Invoke(propertyName, IsSingleContentPage);
        }
    }

}
