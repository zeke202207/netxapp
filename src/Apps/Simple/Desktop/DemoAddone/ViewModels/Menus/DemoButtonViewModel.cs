using Avalonia.Controls;
using DemoAddone.Menus;
using Material.Icons;
using NetX.AppCore.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoAddone
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
