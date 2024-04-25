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
using Serilog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DemoAddone.ViewModels
{
    [ViewModel(ServiceLifetime.Transient)]
    public partial class MainViewModel : MenuPageViewModel
    {
        /// <summary>
        /// 全局唯一标识
        /// </summary>
        public static Guid Id = new Guid("AA000000-0000-0000-0000-000000000001");

        private readonly IFileExplorerManager _fileExplorerManager;
        private readonly BilibiliDataContext _dbContext;
        private ExportType _exportType = ExportType.Floder;

        public ExportType ExportType
        {
            get => _exportType;
            set => this.RaiseAndSetIfChanged(ref _exportType, value);
        }

        public ObservableCollection<CategoryModel> CurrentDirectoryContents { get; set; } = new();
        public ObservableCollection<BreadCrumbItem> BreadCrumbs { get; set; } = new();

        public ReactiveCommand<BreadCrumbItem, Unit> BreadCrumbCommand { get; }
        public ReactiveCommand<CategoryModel, Unit> CategoryCommand { get; }

        public MainViewModel(
            IServiceProvider serviceProvider, 
            IFileExplorerManager fileExplorerManager,
            BilibiliDataContext bilibiliDataContext)
            : base(MainViewModel.Id, serviceProvider, typeof(MainView))
        {
            ReleasePlayerCommand = ReactiveCommand.Create(()=> ReleasePlayer());
            BreadCrumbCommand = ReactiveCommand.Create<BreadCrumbItem>(item => BreadCrumbClick(item));
            CategoryCommand = ReactiveCommand.Create<CategoryModel>(item => CategoryClick(item));
            _fileExplorerManager = fileExplorerManager;
            _dbContext = bilibiliDataContext;
            var fileViewModels = GetDirectoryContents("/Home");
            CurrentDirectoryContents.AddRange(fileViewModels);
            BreadCrumbs.Add(new BreadCrumbItem() { Name = "Home", Path = "/Home" });
        }

        private void CategoryClick(CategoryModel item)
        {
            try
            {
                switch (item.ExportType)
                {
                    case ExportType.Floder:
                        OpenFolder($"{item.ParentPath}/{item.Name}");
                        break;
                    case ExportType.Mp4:
                        OpenVideo($"{item.ParentPath}/{item.Name}", item.ExportType);
                        break;
                    default:
                        throw new NotSupportedException($"不支持的文件格式:{item.ExportType.ToString()}");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "打开失败");
            }            
        }

        private IEnumerable<CategoryModel> GetDirectoryContents(string path)
        {
            if (!Design.IsDesignMode)
            {
                var fileViewModels = _dbContext?.Categories
                .Where(p => p.ParentPath == path)
                .Select(p => new CategoryModel(p.Name, p.CategoryType == 0 ? ExportType.Floder : ExportType.Mp4) { ParentPath = p.ParentPath });
                return fileViewModels;
            }
            else
                return Enumerable.Empty<CategoryModel>();
        }

        /// <summary>
        /// 点击面包屑导航
        /// </summary>
        /// <param name="path"></param>
        private void BreadCrumbClick(BreadCrumbItem item)
        {
            OpenFolder($"{item.Path}");
        }

        /// <summary>
        /// 重构面包屑导航
        /// </summary>
        /// <param name="path"></param>
        private void GenBreadCrumbsNav(string path, bool canClick)
        {
            BreadCrumbs.Clear();
            var paths = path.Split('/');
            //添加到Breadcrumb导航中
            for (int i = 0; i < paths.Length; i++)
            {
                if (string.IsNullOrEmpty(paths[i]))
                    continue;
                var item = new BreadCrumbItem()
                {
                    Name = paths[i],
                    Path = $"{string.Join("/", paths.Take(i + 1))}",
                    IsLast = i == paths.Length - 1,
                };
                if(item.IsLast)
                    item.CanClick = canClick;
                BreadCrumbs.Add(item);
            }
        }

        /// <summary>
        /// 打开文件夹 
        /// </summary>
        /// <param name="path"></param>
        private void OpenFolder(string path)
        {
            ExportType =  ExportType.Floder;
            GenBreadCrumbsNav(path, true);
            var fileViewModels = GetDirectoryContents($"{path}");
            CurrentDirectoryContents.Clear();
            CurrentDirectoryContents.AddRange(fileViewModels);
        }

        /// <summary>
        /// 打开视频文件
        /// </summary>
        /// <param name="videoFile"></param>
        private void OpenVideo(string videoFile, ExportType videoType)
        {
            ExportType = videoType;
            CurrentDirectoryContents.Clear();
            GenBreadCrumbsNav(videoFile, false);
            Play(Path.GetFileName(videoFile));
        }

        public override Control CreateView(IControlCreator controlCreator, Type pageView)=> controlCreator.CreateControl(pageView);

    }
}
