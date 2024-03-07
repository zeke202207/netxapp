using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using NetX.AppCore.Contract;
using NetX.AppCore.ViewModels;
using SukiUI.Controls;
using SukiUI.Models;
using System;
using System.Reactive.Linq;

namespace NetX.AppCore.Views
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

        protected override void OnDataContextEndUpdate()
        {
            base.OnDataContextEndUpdate();
            AttachKeyBindings();
        }

        private void AttachKeyBindings()
        {
            if(DataContext is not MainViewModel _viewModel) 
                return;
            if (!KeyBindings.Exists(p => p.Gesture is KeyGesture k && k.Key == Key.Escape))
            {
                KeyBindings.Add(new KeyBinding
                {
                    Gesture = new KeyGesture(Key.Escape),
                    Command = _viewModel.ExitFullScreenCommand
                });
            }
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

        private void Avatar_Tapped(object? sender, Avalonia.Input.TappedEventArgs e)
        {
            if (DataContext is not MainViewModel vm) 
                return;
            var task = vm.UserDetailCommand?.Execute();
            task.GetAwaiter().GetResult();
        }
    }
}
