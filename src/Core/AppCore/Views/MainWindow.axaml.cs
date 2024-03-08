using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using NetX.AppCore.Contract;
using NetX.AppCore.ViewModels;
using Serilog;
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
            try
            {
                base.OnDataContextEndUpdate();
                AttachKeyBindings();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "���������������ĸ��³���");
            }
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
            try
            {
                if (DataContext is not MainViewModel vm)
                    return;
                if (e.Source is not MenuItem mItem)
                    return;
                if (mItem.DataContext is not SukiColorTheme cTheme)
                    return;
                vm.ChangeTheme(cTheme);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"�޸�����ɫ����");
            }
        }

        private void Avatar_Tapped(object? sender, Avalonia.Input.TappedEventArgs e)
        {
            try
            {
                if (DataContext is not MainViewModel vm)
                    return;
                var task = vm.UserDetailCommand?.Execute();
                task.GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "���ͷ�����");
            }
        }

        private void MenuExpandedChanged(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            try
            {
                if(DataContext is not MainViewModel vm || e.Source is not SukiSideMenu menu)
                    return;
                vm.AvatarSize = menu.IsMenuExpanded ? 80 : 40;
                vm.FooterOrientation = menu.IsMenuExpanded ? Orientation.Horizontal : Orientation.Vertical;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "�˵�չ���������");
            }
        }
    }
}
