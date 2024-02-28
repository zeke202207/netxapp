using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Material.Icons;
using Microsoft.Extensions.Options;
using NetX.AppContainer.Contract;
using NetX.AppContainer.Models;
using NetX.AppContainer.Views;
using SukiUI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetX.AppContainer.ViewModels
{
    [SortIndex(MainViewModel.Order)]
    [ViewModel(ServiceLifetime.Singleton)]
    public class MainViewModel : StartupWindowViewModel
    {
        public const int Order = int.MaxValue;

        public IAvaloniaReadOnlyList<IMenuPageViewModel> Menus { get; }

        public MainViewModel(
            IOptions<AppConfig> option, 
            IControlCreator controlCreator,
            IEnumerable<IMenuPageViewModel> pages)
            : base(controlCreator,typeof(MainWindow), MainViewModel.Order)
        {
            Menus = new AvaloniaList<IMenuPageViewModel>(pages.OrderBy(p => p.Order));
        }

        public override Control CreateView(IControlCreator controlCreator, Type pageView) => controlCreator.CreateControl(pageView);

        protected override void ControlLoaded()
        {
            if (null != Window)
                Window.Closed += (s, e) => base.GotoWindow(-1);
            base.ControlLoaded();
        }
    }
}
