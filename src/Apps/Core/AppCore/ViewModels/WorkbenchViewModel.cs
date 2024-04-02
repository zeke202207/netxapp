using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using DynamicData;
using FluentAvalonia.UI.Controls;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using NetX.AppCore.Contract;
using NetX.AppCore.Contract.ViewModels;
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
    [ViewModel(Contract.ServiceLifetime.Transient)]
    public class WorkbenchViewModel : StartupWindowViewModel
    {
        /// <summary>
        /// 全局唯一标识
        /// </summary>
        public static Guid Id = new Guid("00000000-0000-0000-0000-000000000003");

        private readonly IDataTemplate _dataTemplate;
        private readonly IServiceProvider _serviceProvider;
        private readonly AppUserConfig _appUserConfig;

        #region Command

        public ReactiveCommand<DocumentItem, Unit> TabViewSelectedChangedCommand { get; }
        public ReactiveCommand<NavigationMenu, Unit> NavigationMenuSelectedCommand { get; }

        public ReactiveCommand<DocumentItem,Unit> TabViewItemClosedCommand { get; }

        public ReactiveCommand<Unit, Unit> ExitAppCommand { get; }

        #endregion

        #region 依赖属性

        private Bitmap _avatar;
        public Bitmap Avatar
        {
            get => _avatar;
            set => this.RaiseAndSetIfChanged(ref _avatar, value);
        }

        private List<IFlyoutMenuViewModel> _flyoutMenus;
        public List<IFlyoutMenuViewModel> FlyoutMenus
        {
            get => _flyoutMenus;
            set => this.RaiseAndSetIfChanged(ref _flyoutMenus, value);
        }

        #region navigation menu

        public List<NavigationMenu> NavigationMenu { get; }

        private NavigationMenu _selectedCategory;
        public NavigationMenu SelectedCategory
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

        private int _openPanelWidth = 260;
        public int OpenPanelWidth
        {
            get => _openPanelWidth;
            set => this.RaiseAndSetIfChanged(ref _openPanelWidth, value);
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
            set =>this.RaiseAndSetIfChanged(ref _selectedItem, value);
        }

        #endregion

        #region single view

        private Control _currentPage;
        public Control CurrentPage
        {
            get => _currentPage;
            set => this.RaiseAndSetIfChanged(ref _currentPage, value);
        }

        #endregion

        private bool _isSingleView = false;
        public bool IsSingleView
        {
            get => _isSingleView;
            set => this.RaiseAndSetIfChanged(ref _isSingleView,value);
        }

        #endregion

        public WorkbenchViewModel(
            IDataTemplate dataTemplate, 
            IServiceProvider serviceProvider,
            IOptions<AppAddoneConfig> addoneOptions,
            IOptions<AppUserConfig> userConfig)
            : base(WorkbenchViewModel.Id,serviceProvider, typeof(WorkbenchWindow))
        {
            FlyoutMenus = serviceProvider.GetServices<IFlyoutMenuViewModel>()?.ToList();
            FlyoutMenus.ForEach(p => p.StartupWindow = this);

            _serviceProvider = serviceProvider;
            _dataTemplate = dataTemplate;
            _appUserConfig = userConfig.Value;
            IsSingleView = _appUserConfig.Layouts.Navigationview.IsSingleContentPage;
            OpenPanelWidth = _appUserConfig.Layouts.Navigationview.OpenPaneLength;
            _appUserConfig.Layouts.Navigationview.PropertyHasChanged += (propertyName, value) => LayoutPropertyChaned(propertyName, value);
            
            NavigationMenu = InitNavigateMenu(addoneOptions.Value.NavigationMenuConfig);
            SelectedCategory = NavigationMenu.FirstOrDefault();
            TabViewSelectedChangedCommand = ReactiveCommand.Create<DocumentItem>(item => TabViewSelectedChanged(item));
            NavigationMenuSelectedCommand = ReactiveCommand.Create<NavigationMenu>(menu => NavigationMenuSelected(menu));
            TabViewItemClosedCommand = ReactiveCommand.Create<DocumentItem>(item => TabViewItemClosed(item));
            ExitAppCommand = ReactiveCommand.Create(ExitApp);
            
            _avatar = new Bitmap(AssetLoader.Open(new Uri(@"avares://NetX.AppCore/Assets/default_avatar.png")));
        }

        #region 重写方法

        public override Control CreateView(IControlCreator controlCreator, Type pageView) => controlCreator.CreateControl(pageView);

        #endregion

        #region 私有方法

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        private void LayoutPropertyChaned(string propertyName, object value)
        {
            try
            {
                if (propertyName == nameof(_appUserConfig.Layouts.Navigationview.IsSingleContentPage))
                {
                    IsSingleView = (bool)value;
                    ((ViewLocator)_dataTemplate).ClearCache();
                    NavigationMenuSelected(WorkbenchViewModel.SettingMenu);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "布局属性变更失败");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void TabViewItemClosed(DocumentItem item)
        {
            DocumentItems.Remove(item);
        }

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="menu"></param>
        private void NavigationMenuSelected(NavigationMenu menu)
        {
            var sm = _serviceProvider.GetService(Type.GetType(menu.ViewModelType)) as IViewModel;
            var control = _dataTemplate.Build(sm);
            if (!IsSingleView)
            {
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
            else
            {
                CurrentPage = control;
            }
        }

        /// <summary>
        /// 设置当前页面
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void SetCurrentPage()
        {
            if (null == SelectedCategory || !SelectedCategory.TriggerInvoked) 
                return;
            if (null == SelectedCategory.ViewModelType)
                throw new ArgumentException($"viewtype为空，不能创建页面");
            var sm = _serviceProvider.GetService(Type.GetType(SelectedCategory.ViewModelType)) as IViewModel;
            sm.Key = SelectedCategory.Id;
            var contentPage = _dataTemplate.Build(sm);
            OpenTabview(sm, contentPage);
            CurrentPage = contentPage;
        }

        private void OpenTabview(IViewModel menuPageViewModel,Control control)
        {
            if (!IsSingleView)
            {
                var contentPage = DocumentItems.FirstOrDefault(p => p.NavigateMenuId == menuPageViewModel.Key);
                if (null == contentPage)
                {
                    var navMenu = FindMenuByViewModel(NavigationMenu, menuPageViewModel.Key);
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
            if (null != navMenu)
            {
                if (SelectedCategory?.Id == documentItem.NavigateMenuId)
                    return;
                else
                    SelectedCategory = navMenu;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void ExitApp()
        {
            try
            {
                base.CloseApplication();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "退出应用失败");
            }
        }

        ///// <summary>
        ///// 
        ///// </summary>
        //private void ReLogin()
        //{
        //    try
        //    {
                
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex, "重新登录失败");
        //    }
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        //private void ChangePassword()
        //{
        //    try
        //    {

        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex, "修改密码是吧");
        //    }
        //}

        #endregion

        #region 

        public static NavigationMenu UserMenu = new ViewModels.NavigationMenu()
        {
            Id = GetId(MenuType.User),
            Icon = GetIcon(MenuType.User),
            Title = GetTitle(MenuType.User),
            ViewModelType = GetViewModel(MenuType.User)
        };
        public static NavigationMenu SettingMenu = new ViewModels.NavigationMenu()
        {
            Id = GetId(MenuType.Setting),
            Icon = GetIcon(MenuType.Setting),
            Title = GetTitle(MenuType.Setting),
            ViewModelType = GetViewModel(MenuType.Setting)
        };


        private static string GetTitle(MenuType menuType)
        {
            return menuType switch
            {
                MenuType.User => "用户管理",
                MenuType.Setting => "自定义设置",
                _ => ""
            };
        }

        private static Guid GetId(MenuType menuType)
        {
            return menuType switch
            {
                MenuType.User => new Guid("00000000-abcd-0000-0000-000000000001"),
                MenuType.Setting => new Guid("00000000-abcd-0000-0000-000000000002"),
                _ => Guid.Empty
            };
        }

        private static Symbol GetIcon(MenuType menuType)
        {
            return menuType switch
            {
                MenuType.User => Symbol.Contact,
                MenuType.Setting => Symbol.DevelopTools,
                _ => Symbol.ContactPresence
            };
        }

        private static string GetViewModel(MenuType menuType)
        {
            return menuType switch
            {
                MenuType.User => "NetX.AppCore.ViewModels.TestViewModel",
                MenuType.Setting => "NetX.AppCore.ViewModels.SettingPageViewModel",
                _ => ""
            };
        }


        enum MenuType
        {
            User,
            Setting
        }

        #endregion
    }
}
