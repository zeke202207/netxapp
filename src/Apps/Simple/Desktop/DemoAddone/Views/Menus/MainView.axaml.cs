using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using DemoAddone.RPCService;
using DemoAddone.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NetX.Controls.Media;
using Serilog;
using System.Collections.ObjectModel;
using System.Reactive.Linq;

namespace DemoAddone.Views.Menus
{
    public partial class MainView : UserControl
    {
        public MainView()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void RepeatButton_Tapped(object? sender, Avalonia.Input.TappedEventArgs e)
        {
            try
            {
                if(this.DataContext is MainViewModel fileExplorerViewModel && sender is RepeatButton btn)
                {
                    if (btn.Tag is BreadCrumbItem breadCrumbItem)
                    {
                        ReleaseResource();
                        fileExplorerViewModel.BreadCrumbCommand.Execute(breadCrumbItem)
                            .GetAwaiter().GetResult();
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Ãæ°üÐ¼µ¼º½Ê§°Ü");
            }
        }

        protected override void OnUnloaded(RoutedEventArgs e)
        {
            ReleaseResource();
            base.OnUnloaded(e);
        }

        private void ReleaseResource()
        {
            if (this.DataContext is MainViewModel fileExplorerViewModel)
            {
                fileExplorerViewModel.ReleasePlayerCommand.Execute().GetAwaiter().GetResult();
            }
        }
    }
}
