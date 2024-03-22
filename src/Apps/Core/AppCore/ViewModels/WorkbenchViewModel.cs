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
using System.Linq;

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

        #region 依赖属性

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

        private Control _currentPage;
        public Control CurrentPage
        {
            get => _currentPage;
            set => this.RaiseAndSetIfChanged(ref _currentPage, value);
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
                    Title = menuConfig.Title,
                    ToolTip = menuConfig.Tooltip,
                    Icon = Enum.Parse<Symbol>(menuConfig.Icon),
                    ViewType = menuConfig.ViewModel,
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
                if(null == cat.ViewType)
                    throw new ArgumentException($"viewtype为空，不能创建页面");
                var sm = _serviceProvider.GetService(Type.GetType(cat.ViewType));
                CurrentPage = _dataTemplate.Build(sm);
            }
            else if (SelectedCategory is NavigationViewItem nvi)
            {
                var smpPage = $"NetX.AppCore.ViewModels.SettingPageViewModel";
                var sm = _serviceProvider.GetService(Type.GetType(smpPage));
                CurrentPage = _dataTemplate.Build(sm);
            }
        }

        #endregion

    }
}
