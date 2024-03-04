using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using NetX.AppContainer.ViewModels;
using SukiUI.Controls;
using SukiUI.Models;

namespace NetX.AppContainer.Views
{
    public partial class MainWindow : SukiWindow
    {
        public MainWindow()
        {
#if DEBUG
            this.AttachDevTools();
#endif
            AvaloniaXamlLoader.Load(this);
        }

        private void MenuItem_OnClick(object? sender, RoutedEventArgs e)
        {
            if (DataContext is not MainViewModel vm) 
                return;
            if (e.Source is not MenuItem mItem) 
                return;
            if (mItem.DataContext is not SukiColorTheme cTheme) 
                return;
            vm.ChangeTheme(cTheme);
        }
    }
}
