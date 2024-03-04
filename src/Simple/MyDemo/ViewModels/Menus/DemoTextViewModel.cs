using Avalonia.Controls;
using Material.Icons;
using MyDemo.Views.Menus;
using NetX.AppContainer.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDemo.ViewModels.Menus
{
    [SortIndex(LoginViewModel.Order , false)]
    [ViewModel(ServiceLifetime.Singleton)]
    public class DemoTextViewModel : MenuPageViewModel
    {
        public DemoTextViewModel(IControlCreator controlCreator)
            : base(controlCreator, typeof(DemoTextView), "文本框示例", MaterialIconKind.Text, 1)
        {

        }

        public override Control CreateView(IControlCreator controlCreator, Type pageView) => controlCreator.CreateControl(pageView);
    }
}
