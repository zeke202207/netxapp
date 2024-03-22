using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using FluentAvalonia.UI.Controls;
using FluentAvalonia.UI.Windowing;
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
                    workbenchViewModel.NavigationMenuSelectedCommand.Execute(new ViewModels.NavigationMenu()
                    {
                        Id = GetId(nvi.Name),
                        Icon = GetIcon(nvi.Name),
                        Title = nvi.Content.ToString(),
                        ViewModelType = nvi.Tag.ToString()
                    })
                        .GetAwaiter().GetResult();
                }
            }
        }

        private Guid GetId(string name)
        {
            return name switch
            {
                "user" => new Guid("00000000-abcd-0000-0000-000000000001"),
                "setting" => new Guid("00000000-abcd-0000-0000-000000000002"),
                _ => Guid.Empty
            };
        }

        private Symbol GetIcon(string name)
        {
            return name switch
            {
                "user" =>  Symbol.Contact,
                "setting" =>  Symbol.Settings ,
                _ => Symbol.Emoji
            };
        }
    }
}
