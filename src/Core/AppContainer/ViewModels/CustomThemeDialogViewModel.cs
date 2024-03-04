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

namespace NetX.AppContainer.ViewModels
{
    [ViewModel(ServiceLifetime.Singleton)]
    public partial class CustomThemeDialogViewModel : BaseViewModel
    {
        private string _displayName = "Pink";
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

        public ReactiveCommand<Unit, Unit> TryCreateThemeCommand { get; }

        public CustomThemeDialogViewModel(SukiTheme theme, IControlCreator controlCreator)
            : base(controlCreator, typeof(CustomThemeDialogView))
        {
            _theme = theme;
            TryCreateThemeCommand = ReactiveCommand.Create(TryCreateTheme);
        }

        public void TryCreateTheme()
        {
            if (string.IsNullOrEmpty(DisplayName)) 
                return;
            var theme = new SukiColorTheme(DisplayName, PrimaryColor, AccentColor);
            _theme.AddColorTheme(theme);
            _theme.ChangeColorTheme(theme);
            SukiHost.CloseDialog();
        }

        public override Control CreateView(IControlCreator controlCreator, Type pageView) => controlCreator.CreateControl(pageView);
    }
}
