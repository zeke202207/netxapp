using Avalonia.Media;
using SukiUI.Controls;
using SukiUI.Models;
using SukiUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetX.AppContainer.Contract;
using Avalonia.Controls;
using NetX.AppContainer.Views;
using ReactiveUI;
using System.Reactive;
using Microsoft.Extensions.Options;
using NetX.AppContainer.Models;
using System.Xml.Linq;

namespace NetX.AppContainer.ViewModels
{
    [ViewModel(ServiceLifetime.Singleton)]
    public partial class CustomThemeDialogViewModel : BaseViewModel
    {
        public Action<SukiColorTheme> OnColorThemeChanged;

        private string _displayName = "zeke";
        public string DisplayName
        {
            get => _displayName;
            set => this.RaiseAndSetIfChanged(ref _displayName, value);
        }

        private Color _primaryColor = Colors.DeepPink;
        public Color PrimaryColor
        {
            get => _primaryColor;
            set => this.RaiseAndSetIfChanged(ref _primaryColor, value);
        }

        private Color _accentColor = Colors.Pink;
        public Color AccentColor
        {
            get => _accentColor;
            set => this.RaiseAndSetIfChanged(ref _accentColor, value);
        }

        private readonly SukiTheme _theme;
        private readonly AppConfig _option;

        public ReactiveCommand<Unit, Unit> TryCreateThemeCommand { get; }

        public CustomThemeDialogViewModel(SukiTheme theme, IControlCreator controlCreator,
            IOptions<AppConfig> option)
            : base(controlCreator, typeof(CustomThemeDialogView))
        {
            _option = option.Value;
            _theme = theme;
            TryCreateThemeCommand = ReactiveCommand.Create(TryCreateTheme);
        }

        public void TryCreateTheme()
        {
            if (string.IsNullOrEmpty(DisplayName)) 
                return;
            ChangeColorTheme();
            Save2Config();
            SukiHost.CloseDialog();
        }

        private void ChangeColorTheme()
        {
            var theme = new SukiColorTheme(DisplayName, PrimaryColor, AccentColor);
            _theme.AddColorTheme(theme);
            _theme.ChangeColorTheme(theme);
            OnColorThemeChanged?.Invoke(theme);
        }

        private void Save2Config()
        {
            _option.Themes.ThemeColor.DisplayName = DisplayName;
            _option.Themes.ThemeColor.Primary = PrimaryColor.ToString();
            _option.Themes.ThemeColor.Accent = AccentColor.ToString();
            _option.Save();
        }

        public override Control CreateView(IControlCreator controlCreator, Type pageView) => controlCreator.CreateControl(pageView);
    }
}
