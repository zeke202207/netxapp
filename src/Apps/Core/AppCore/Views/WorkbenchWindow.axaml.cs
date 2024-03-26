using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using FluentAvalonia.UI.Controls;
using FluentAvalonia.UI.Windowing;
using NetX.AppCore.ViewModels;
using System;
using System.Reactive.Linq;

namespace NetX.AppCore.Views
{
    public partial class WorkbenchWindow : AppWindow
    {
        public WorkbenchWindow()
        {
            AvaloniaXamlLoader.Load(this);
#if DEBUG
            this.AttachDevTools();
#endif

        }

        private void CloseTabViewItem(TabViewItem item, TabViewTabCloseRequestedEventArgs args)
        {
            if (this.DataContext is ViewModels.WorkbenchViewModel workbenchViewModel)
                workbenchViewModel.TabViewItemClosedCommand.Execute(args.Item as DocumentItem)
                    .GetAwaiter().GetResult();
        }

        private void NavigateTabView_SelectionChanged(object sender, Avalonia.Controls.SelectionChangedEventArgs args)
        {
            if(this.DataContext is ViewModels.WorkbenchViewModel workbenchViewModel)
            {
                if(args.AddedItems.Count > 0)
                {
                    var item = args.AddedItems[0] as ViewModels.DocumentItem;
                    workbenchViewModel.TabViewSelectedChangedCommand.Execute(item)
                        .GetAwaiter().GetResult();
                }
            }
        }

        private void NavigationViewItem_PointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
        {
            if (sender is FluentAvalonia.UI.Controls.NavigationViewItem nvi)
            {
                if (this.DataContext is ViewModels.WorkbenchViewModel workbenchViewModel)
                {
                    var menu = nvi.Name.ToLower() == "user" ? WorkbenchViewModel.UserMenu : WorkbenchViewModel.SettingMenu;
                    workbenchViewModel.NavigationMenuSelectedCommand.Execute(menu)
                        .GetAwaiter().GetResult();
                }
            }
        }
    }
}
