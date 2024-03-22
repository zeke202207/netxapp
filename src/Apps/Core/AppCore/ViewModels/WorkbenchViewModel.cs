using Avalonia.Controls;
using Avalonia.Controls.Templates;
using DynamicData;
using FluentAvalonia.UI.Controls;
using Microsoft.Extensions.Options;
using NetX.AppCore.Contract;
using NetX.AppCore.Models;
using NetX.AppCore.Views;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetX.AppCore.ViewModels
{
    [ViewModel(ServiceLifetime.Singleton)]
    public class WorkbenchViewModel : StartupWindowViewModel
    {
        /// <summary>
        /// 全局唯一标识
        /// </summary>
        public static Guid Id = new Guid("00000000-0000-0000-0000-000000000003");

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

        private List<NavigationMenu> InitNavigateMenu(NavigationMenuConfig[] menus)
        {
            return menus.Select(menuConfig => new NavigationMenu
            {
                Id = new Guid(menuConfig.Id),
                ParentId = new Guid(menuConfig.ParentId),
                Title = menuConfig.Title,
                ToolTip = menuConfig.Tooltip,
                Icon = GetIcon(menuConfig.Icon),
                ViewType = menuConfig.ViewModel,
                TriggerInvoked = menuConfig.TriggerInvoked,
                ChildMenu = menuConfig.Childrens != null && menuConfig.Childrens.Length > 0 ? InitNavigateMenu(menuConfig.Childrens) : null
            }).ToList();
        }

        private Symbol GetIcon(string icon)
        {
            return Enum.Parse<Symbol>(icon);
        }

        public override Control CreateView(IControlCreator controlCreator, Type pageView) => controlCreator.CreateControl(pageView);

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
    }
}
