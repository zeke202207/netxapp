using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using NetX.AppCore.ViewModels;
using System.Runtime.CompilerServices;

namespace NetX.AppCore.Views
{
    public partial class SettingPage : UserControl
    {
        public SettingPage()
        {
            AvaloniaXamlLoader.Load(this);
            if (Design.IsDesignMode)
                DataContext = new NetX.AppCore.ViewModels.SettingPageViewModel(null, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoaded(RoutedEventArgs e)
        {
            base.OnLoaded(e);

            var dc = DataContext as SettingPageViewModel;

            if (TryGetResource("SystemAccentColor", null, out var value))
            {
                var color = Unsafe.Unbox<Color>(value);
                dc.CustomAccentColor = color;
                dc.ListBoxColor = color;
            }
        }
    }
}
