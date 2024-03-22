using Avalonia.Controls;
using Avalonia.Controls.Templates;
using DynamicData;
using FluentAvalonia.UI.Controls;
using Microsoft.Extensions.Options;
using NetX.AppCore.Contract;
using NetX.AppCore.Models;
using NetX.AppCore.Views;
using ReactiveUI;
using Serilog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive;

namespace NetX.AppCore.ViewModels
{
    /// <summary>
    /// 工作太窗口视图模型
    /// </summary>
    [ViewModel(ServiceLifetime.Singleton)]
    public class WorkbenchViewModel : StartupWindowViewModel
    {
        /// <summary>
        /// 全局唯一标识
        /// </summary>
        public static Guid Id = new Guid("00000000-0000-0000-0000-000000000003");

        #region Command

        public ReactiveCommand<DocumentItem, Unit> TabViewSelectedChangedCommand { get; }
        public ReactiveCommand<NavigationMenu, Unit> NavigationMenuSelectedCommand { get; }

        #endregion

        #region 依赖属性

        #region navigation menu

        private readonly IDataTemplate _dataTemplate;
        private readonly IServiceProvider _serviceProvider;

        public List<NavigationMenu> NavigationMenu { get; }

        private object _selectedCategory;
        public object SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedCategory, value);
                SetCurrentPage();
            }
        }

        private NavigationViewPaneDisplayMode _nVDisplayMode = NavigationViewPaneDisplayMode.Left;
        public NavigationViewPaneDisplayMode NVDisplayMode
        {
            get => _nVDisplayMode;
            set => this.RaiseAndSetIfChanged(ref _nVDisplayMode, value);
        }

        private bool _nvCanToggle = true;
        public bool NVCanToggle
        {
            get => _nvCanToggle;
            set => this.RaiseAndSetIfChanged(ref _nvCanToggle, value);
        }

        #endregion

        #region tabview

        private ObservableCollection<DocumentItem> _documentItems=new ObservableCollection<DocumentItem>();
        public ObservableCollection<DocumentItem> DocumentItems
        {
            get => _documentItems;
            set => this.RaiseAndSetIfChanged(ref _documentItems, value);
        }

        private DocumentItem _selectedItem;
        public DocumentItem SelectedItem
        {
            get => _selectedItem;
            set => this.RaiseAndSetIfChanged(ref _selectedItem, value);
        }

        #endregion

        #endregion

        public WorkbenchViewModel(
            IDataTemplate dataTemplate, 
            IServiceProvider serviceProvider,
            IOptions<AppAddoneConfig> addoneOptions)
            : base(WorkbenchViewModel.Id,serviceProvider, typeof(WorkbenchWindow))
        {
            _serviceProvider = serviceProvider;
            _dataTemplate = dataTemplate;
            _serviceProvider = serviceProvider;
            NavigationMenu = InitNavigateMenu(addoneOptions.Value.NavigationMenuConfig);
            SelectedCategory = NavigationMenu.FirstOrDefault();
            TabViewSelectedChangedCommand = ReactiveCommand.Create<DocumentItem>(item => TabViewSelectedChanged(item));
            NavigationMenuSelectedCommand = ReactiveCommand.Create<NavigationMenu>(menu => NavigationMenuSelected(menu));

            //DocumentItems = new List<DocumentItem>()
            //{
            //    new DocumentItem(){ Header = "Tab1", IconSource = new SymbolIconSource { Symbol = Symbol.Star }, Content = new TextBlock(){ Text = "Tab1" } },
            //    new DocumentItem(){ Header = "Tab2", IconSource = new SymbolIconSource { Symbol = Symbol.Home }, Content = new TextBlock(){ Text = "Tab2" } },
            //    new DocumentItem(){ Header = "Tab3", IconSource = new SymbolIconSource { Symbol = Symbol.ZoomInFilled }, Content = new TextBlock(){ Text = "Tab3" } }
            //};

            //SelectedItem = DocumentItems[1];
        }

        #region 重写方法

        public override Control CreateView(IControlCreator controlCreator, Type pageView) => controlCreator.CreateControl(pageView);

        #endregion

        #region 私有方法

        /// <summary>
        /// 初始化导航菜单
        /// </summary>
        /// <param name="menus"></param>
        /// <returns></returns>
        private List<NavigationMenu> InitNavigateMenu(NavigationMenuConfig[] menus)
        {
            try
            {
                return menus.Select(menuConfig => new NavigationMenu
                {
                    Id = new Guid(menuConfig.Id),
                    ParentId = new Guid(menuConfig.ParentId),
                    Order = menuConfig.Order,
                    Title = menuConfig.Title,
                    ToolTip = menuConfig.Tooltip,
                    Icon = Enum.Parse<Symbol>(menuConfig.Icon),
                    ViewModelType = menuConfig.ViewModel,
                    TriggerInvoked = menuConfig.TriggerInvoked,
                    ChildMenu = menuConfig.Childrens != null && menuConfig.Childrens.Length > 0 ? InitNavigateMenu(menuConfig.Childrens) : null
                }).ToList();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "获取菜单失败");
                return new List<NavigationMenu>();
            }
        }


        private void NavigationMenuSelected(NavigationMenu menu)
        {
            var sm = _serviceProvider.GetService(Type.GetType(menu.ViewModelType)) as IViewModel;
            var control = _dataTemplate.Build(sm);
            var contentPage = DocumentItems.FirstOrDefault(p => p.NavigateMenuId == menu.Id);
            if (null == contentPage)
            {
                contentPage = new DocumentItem()
                {
                    NavigateMenuId = menu.Id,
                    Header = menu.Title,
                    IconSource = new SymbolIconSource { Symbol = menu.Icon },
                    Content = control,
                    IsClosable = true
                };
                DocumentItems.Add(contentPage);
            }
            SelectedItem = contentPage;
        }

        /// <summary>
        /// 设置当前页面
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void SetCurrentPage()
        {
            if (SelectedCategory is NavigationMenu cat)
            {
                if (null == cat) return;
                if (!cat.TriggerInvoked)
                    return;
                if(null == cat.ViewModelType)
                    throw new ArgumentException($"viewtype为空，不能创建页面");
                var sm = _serviceProvider.GetService(Type.GetType(cat.ViewModelType)) as IViewModel;
                sm.Key = cat.Id;
                var contentPage = _dataTemplate.Build(sm);
                OpenTabview(sm,contentPage);
            }
            //else if (SelectedCategory is NavigationViewItem nvi)
            //{
            //    var smpPage = $"NetX.AppCore.ViewModels.SettingPageViewModel";
            //    var sm = _serviceProvider.GetService(Type.GetType(smpPage)) as IViewModel;
            //    var contentPage = _dataTemplate.Build(sm);
            //}
        }

        private void OpenTabview(IViewModel menuPageViewModel,Control control)
        {
            var contentPage = DocumentItems.FirstOrDefault(p => p.NavigateMenuId == menuPageViewModel.Key);
            if (null == contentPage)
            {
                var navMenu = FindMenuByViewModel(NavigationMenu,menuPageViewModel.Key);
                contentPage = new DocumentItem()
                {
                    NavigateMenuId = menuPageViewModel.Key,
                    Header = navMenu.Title,
                    IconSource = new SymbolIconSource { Symbol = navMenu.Icon },
                    Content = control,
                    IsClosable = !(navMenu.ParentId == Guid.Empty && navMenu.Id == NavigationMenu.FirstOrDefault()?.Id)
                };
                DocumentItems.Add(contentPage);
            }
            SelectedItem = contentPage;
        }

        private NavigationMenu FindMenuByViewModel(List<NavigationMenu> menus, Guid key)
        {
            foreach (var menu in menus)
            {
                if (menu.Id == key)
                    return menu;
                if (menu.ChildMenu != null)
                {
                    NavigationMenu foundMenu = FindMenuByViewModel(menu.ChildMenu, key);
                    if (foundMenu != null)
                        return foundMenu;
                }
            }
            return null;
        }


        /// <summary>
        /// tabview切换选中项目
        /// </summary>
        /// <param name="documentItem"></param>
        private void TabViewSelectedChanged(DocumentItem documentItem)
        {
            var navMenu = FindMenuByViewModel(NavigationMenu, documentItem.NavigateMenuId);
            if(null!= navMenu)
                SelectedCategory = navMenu;
        }

        #endregion

    }
}
