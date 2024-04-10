using Avalonia.Controls;
using Avalonia.Input;
using DemoAddone.Data;
using DemoAddone.RPCService;
using DemoAddone.Views.Menus;
using DynamicData;
using FluentAvalonia.UI.Controls;
using Microsoft.EntityFrameworkCore;
using NetX.AppCore.Contract;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DemoAddone.ViewModels
{
    [ViewModel(ServiceLifetime.Transient)]
    public class FileExplorerViewModel : MenuPageViewModel
    {
        /// <summary>
        /// 全局唯一标识
        /// </summary>
        public static Guid Id = new Guid("AA000000-0000-0000-0000-000000000001");

        private readonly IFileExplorerManager _fileExplorerManager;
        private readonly BilibiliDataContext _dbContext;

        public ObservableCollection<FileViewModel> CurrentDirectoryContents { get; set; } = new();
        public ObservableCollection<BreadCrumbItem> BreadCrumbs { get; set; } = new();

        public ReactiveCommand<BreadCrumbItem, Unit> BreadCrumbCommand { get; }
        public ReactiveCommand<FileViewModel, Unit> CategoryCommand { get; }

        public FileExplorerViewModel(
            IServiceProvider serviceProvider, 
            IFileExplorerManager fileExplorerManager,
            BilibiliDataContext bilibiliDataContext)
            : base(FileExplorerViewModel.Id, serviceProvider, typeof(FileExplorerView))
        {
            BreadCrumbCommand = ReactiveCommand.Create<BreadCrumbItem>(item => BreadCrumbClick(item));
            CategoryCommand = ReactiveCommand.Create<FileViewModel>(item => CategoryClick(item));
            _fileExplorerManager = fileExplorerManager;
            _dbContext = bilibiliDataContext;
            var fileViewModels = GetDirectoryContents("/Home");
            CurrentDirectoryContents.AddRange(fileViewModels);
            BreadCrumbs.Add(new BreadCrumbItem() { Name = "Home", Path = "/Home" });
        }

        private void CategoryClick(FileViewModel item)
        {
            OpenFolder($"{item.ParentPath}/{item.Name}");
        }

        private IEnumerable<FileViewModel> GetDirectoryContents(string path)
        {
            var fileViewModels = _dbContext?.Categories
                .Where(p => p.ParentPath == path)
                .Select(p =>new FileViewModel(p.Name, p.CategoryType == 0 ? ExportType.Floder : ExportType.Mp4) {  ParentPath = p.ParentPath});
            return fileViewModels;
        }

        /// <summary>
        /// 点击面包屑导航
        /// </summary>
        /// <param name="path"></param>
        private void BreadCrumbClick(BreadCrumbItem item)
        {
            OpenFolder($"{item.Path}");
        }

        private void OpenFolder(string path)
        {
            var fileViewModels = GetDirectoryContents($"{path}");
            CurrentDirectoryContents.Clear();
            CurrentDirectoryContents.AddRange(fileViewModels);
            BreadCrumbs.Clear();
            var paths = path.Split('/');
            //添加到Breadcrumb导航中
            for (int i = 0; i < paths.Length; i++)
            {
                if (string.IsNullOrEmpty(paths[i]))
                    continue;
                BreadCrumbs.Add(new BreadCrumbItem() { Name = paths[i], Path = $"{string.Join("/", paths.Take(i + 1))}" , IsLast = i== paths.Length -1 });
            }
        }

        public override Control CreateView(IControlCreator controlCreator, Type pageView)=> controlCreator.CreateControl(pageView);

    }
}
