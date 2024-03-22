using Avalonia.Controls;
using Avalonia.Controls.Templates;
using DynamicData;
using FluentAvalonia.UI.Controls;
using NetX.AppCore.Contract;
using NetX.AppCore.Views;
using ReactiveUI;
using System;
using System.Collections.Generic;

namespace NetX.AppCore.ViewModels
{
    [SortIndex(WorkbenchViewModel.Order)]
    [ViewModel(ServiceLifetime.Singleton)]
    public class WorkbenchViewModel : StartupWindowViewModel
    {
        public const int Order = int.MaxValue;
        private readonly IDataTemplate _dataTemplate;
        private readonly IServiceProvider _serviceProvider;

        private object _selectedCategory;

        public List<CategoryBase> Categories { get; }
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

        public WorkbenchViewModel(IDataTemplate dataTemplate, IServiceProvider serviceProvider)
            : base(serviceProvider, typeof(WorkbenchWindow), WorkbenchViewModel.Order)
        {
            _serviceProvider = serviceProvider;
            _dataTemplate = dataTemplate;
            Categories = new List<CategoryBase>();

            Categories.Add(new Category { Name = "Category 1", Icon = Symbol.Home, ToolTip = "This is category 1", ViewType = "NetX.AppCore.ViewModels.TestViewModel", TriggerInvoked = true });
            Categories.Add(new Category { Name = "Category 2", Icon = Symbol.Keyboard, ToolTip = "This is category 2", ViewType = "NetX.AppCore.ViewModels.TestViewModel", TriggerInvoked = true });
            Categories.Add(new Separator());
            Categories.Add(new Category { Name = "Category 3", Icon = Symbol.Library, ToolTip = "This is category 3", ViewType = "NetX.AppCore.ViewModels.TestViewModel", TriggerInvoked = true });
            Categories.Add(new Category
            {
                Name = "Category 4",
                Icon = Symbol.Mail,
                ToolTip = "This is category 4",
                TriggerInvoked = false,
                Children = new List<CategoryBase>()
                    {
                        new Category { Name = "Item 1", Icon = Symbol.Home, ToolTip = "This is item 1", TriggerInvoked = false },
                        new Category { Name = "Item 2", Icon = Symbol.Keyboard, ToolTip = "This is item 2", TriggerInvoked = true , ViewType="NetX.AppCore.ViewModels.TestViewModel" },
                    }
            });
            _serviceProvider = serviceProvider;
        }

        public override Control CreateView(IControlCreator controlCreator, Type pageView) => controlCreator.CreateControl(pageView);

        /// <summary>
        /// 设置当前页面
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void SetCurrentPage()
        {
            if (SelectedCategory is Category cat)
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
