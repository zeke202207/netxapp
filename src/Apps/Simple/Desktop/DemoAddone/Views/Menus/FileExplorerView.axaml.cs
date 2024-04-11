using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using DemoAddone.RPCService;
using DemoAddone.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.Collections.ObjectModel;
using System.Reactive.Linq;

namespace DemoAddone.Views.Menus
{
    public partial class FileExplorerView : UserControl
    {
        public FileExplorerView()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void RepeatButton_Tapped(object? sender, Avalonia.Input.TappedEventArgs e)
        {
            try
            {
                if(this.DataContext is FileExplorerViewModel fileExplorerViewModel && sender is RepeatButton btn)
                {
                    if(btn.Tag is BreadCrumbItem breadCrumbItem)
                        fileExplorerViewModel.BreadCrumbCommand.Execute(breadCrumbItem)
                            .GetAwaiter().GetResult();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Ãæ°üÐ¼µ¼º½Ê§°Ü");
            }
        }
    }
}
