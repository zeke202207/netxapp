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
            if (Design.IsDesignMode)
            {
                this.DataContext = new FileExplorerViewModel(
                    new ServiceCollection().BuildServiceProvider(), 
                    new LocalFileExplorerManager(), 
                    new Data.BilibiliDataContext(null))
                {
                    CurrentDirectoryContents = new System.Collections.ObjectModel.ObservableCollection<FileViewModel>()
                         {
                              new FileViewModel("Folder 1",  ExportType.Floder),
                            new FileViewModel("File 1",  ExportType.Mp4),
                            new FileViewModel("File 2", ExportType.Mp4),
                         },
                    BreadCrumbs = new ObservableCollection<BreadCrumbItem>()
                        {
                            new BreadCrumbItem(){ Name = "Home", Path = "/" },
                            new BreadCrumbItem(){ Name = "Zeke", Path = "/a" },
                            new BreadCrumbItem(){ Name = "Zank", Path = "/a/b" }
                        }
                };
            }
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
                Log.Error(ex, "面包屑导航失败");
            }
        }

        private void Category_Tapped(object? sender , Avalonia.Input.TappedEventArgs e)
        {
            try
            {
                if (this.DataContext is FileExplorerViewModel fileExplorerViewModel && sender is Border border)
                {
                    if (border.Tag is FileViewModel fileViewModel)
                        fileExplorerViewModel.CategoryCommand.Execute(fileViewModel)
                            .GetAwaiter().GetResult();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "打开文件夹失败");
            }
        }
    }
}
