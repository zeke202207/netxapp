using Avalonia.Controls;
using Material.Icons;
using MyDemo.Views.Menus;
using NetX.AppCore.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDemo.ViewModels.Menus
{
    [SortIndex(0)]
    [ViewModel(ServiceLifetime.Singleton)]
    public class DemoButtonViewModel : MenuPageViewModel
    {
        public DemoButtonViewModel(IControlCreator controlCreator)
            : base(controlCreator, typeof(DemoButtonView), "按钮示例", MaterialIconKind.Button, 0)
        {

        }

        public override Control CreateView(IControlCreator controlCreator, Type pageView) => controlCreator.CreateControl(pageView);
    }
}
