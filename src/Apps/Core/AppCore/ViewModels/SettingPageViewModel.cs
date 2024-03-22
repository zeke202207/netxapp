using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Media;
using Avalonia.Styling;
using FluentAvalonia.Styling;
using Microsoft.Extensions.Options;
using NetX.AppCore.Contract;
using NetX.AppCore.Models;
using NetX.AppCore.Views;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetX.AppCore.ViewModels
{
    [ViewModel(ServiceLifetime.Singleton)]
    public class SettingPageViewModel : BaseViewModel
    {
        private readonly AppUserConfig _appUserConfig;

        public string[] AppThemes { get; } =
            new[] { _system, _light, _dark /*, FluentAvaloniaTheme.HighContrastTheme*/ };

        public string CurrentAppTheme
        {
            get => _currentAppTheme;
            set
            {
                if (EqualityComparer<string>.Default.Equals(_currentAppTheme, value))
                    return;
                this.RaiseAndSetIfChanged(ref _currentAppTheme, value);

                var newTheme = GetThemeVariant(value);
                if (newTheme != null)
                {
                    Application.Current.RequestedThemeVariant = newTheme;
                    _appUserConfig.Themes.Theme = newTheme.ToString();
                    _appUserConfig.Save();
                }
                if (value != _system)
                    _faTheme.PreferSystemTheme = false;
                else
                    _faTheme.PreferSystemTheme = true;
            }
        }

        private ThemeVariant GetThemeVariant(string value)
        {
            switch (value)
            {
                case _light:
                    return ThemeVariant.Light;
                case _dark:
                    return ThemeVariant.Dark;
                case _system:
                default:
                    return null;
            }
        }

        public bool UseCustomAccent
        {
            get => _useCustomAccentColor;
            set
            {
                if (EqualityComparer<bool>.Default.Equals(_useCustomAccentColor, value))
                    return;
                this.RaiseAndSetIfChanged(ref _useCustomAccentColor, value);
                if (value)
                {
                    if (_faTheme.TryGetResource("SystemAccentColor", null, out var curColor))
                    {
                        _customAccentColor = (Color)curColor;
                        _listBoxColor = _customAccentColor;

                        this.RaisePropertyChanged(nameof(CustomAccentColor));
                        this.RaisePropertyChanged(nameof(ListBoxColor));
                    }
                    else
                    {
                        // This should never happen, if it does, something bad has happened
                        throw new Exception("Unable to retreive SystemAccentColor");
                    }
                }
                else
                {
                    // Restore system color
                    _customAccentColor = default;
                    _listBoxColor = default;
                    this.RaisePropertyChanged(nameof(CustomAccentColor));
                    this.RaisePropertyChanged(nameof(ListBoxColor));
                    UpdateAppAccentColor(Color.FromRgb(0, 120, 215));
                }
            }
        }

        // This is bound to the ListBox of predefined colors. It must be nullable or CompiledBindings will get angry
        // if we set a color here that isn't in the predef colors as SelectingItemsControl will try to bind back
        // null as the SelectedItem 
        public Color? ListBoxColor
        {
            get => _listBoxColor;
            set
            {
                if (EqualityComparer<Color?>.Default.Equals(_listBoxColor, value))
                    return;
                this.RaiseAndSetIfChanged(ref _listBoxColor, (Color)value);

                if (value != null)
                {
                    _customAccentColor = value.Value;
                    this.RaisePropertyChanged(nameof(CustomAccentColor));

                    UpdateAppAccentColor(value.Value);
                }
            }
        }

        // This is the custom accent color as chosen by the ColorPicker and is not one of the predefined colors
        public Color CustomAccentColor
        {
            get => _customAccentColor;
            set
            {
                if (EqualityComparer<Color>.Default.Equals(_customAccentColor, value))
                    return;
                this.RaiseAndSetIfChanged(ref _customAccentColor, value);
                _listBoxColor = value;
                this.RaisePropertyChanged(nameof(ListBoxColor));
                UpdateAppAccentColor(value);
            }
        }

        public List<Color> PredefinedColors { get; private set; }

        public string CurrentVersion =>
            typeof(FluentAvalonia.UI.Controls.NavigationView).Assembly.GetName().Version?.ToString();

        public string CurrentAvaloniaVersion =>
            typeof(Application).Assembly.GetName().Version?.ToString();

        private void GetPredefColors()
        {
            PredefinedColors = new List<Color>
                {
                    Color.FromRgb(0,120,215),
                    Color.FromRgb(209,52,56),
                    Color.FromRgb(0,99,177),
                    Color.FromRgb(107,105,214),
                    Color.FromRgb(116,77,169),
                    Color.FromRgb(177,70,194),
                    Color.FromRgb(122,117,116),
                    Color.FromRgb(104,118,138),
                    Color.FromRgb(132,117,69),
                };
        }

        private void UpdateAppAccentColor(Color? color)
        {
            _faTheme.CustomAccentColor = color;
            if (null != _appUserConfig)
            {
                _appUserConfig.Themes.IsCustomAccent = UseCustomAccent;
                _appUserConfig.Themes.AccentColor = color.ToString();
                _appUserConfig.Save();
            }
        }

        private bool _useCustomAccentColor;
        private Color _customAccentColor = Colors.SlateBlue;
        private string _currentAppTheme = _system;
        private Color? _listBoxColor;

        private const string _system = "System";
        private const string _dark = "Dark";
        private const string _light = "Light";
        private readonly FluentAvaloniaTheme _faTheme;

        public SettingPageViewModel(IServiceProvider serviceProvider, IOptions<AppUserConfig> option) 
            : base(serviceProvider, typeof(SettingPage))
        {
            _appUserConfig = option.Value;
            GetPredefColors();
            _faTheme = App.Current.Styles[0] as FluentAvaloniaTheme;
            CurrentAppTheme = _appUserConfig.Themes.Theme;
            UseCustomAccent = _appUserConfig.Themes.IsCustomAccent;
        }

        public override Control CreateView(IControlCreator controlCreator, Type pageView) => controlCreator.CreateControl(pageView);
    }
}
