using Avalonia.Controls;
using Material.Icons;
using NetX.AppCore.Contract;
using NetX.RBAC.Views.Menus;
using SukiUI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetX.RBAC.ViewModels.Menus
{
    [SortIndex(2)]
    [ViewModel(ServiceLifetime.Singleton)]
    public class TestZekeViewModel : MenuPageViewModel
    {
        public TestZekeViewModel(IControlCreator controlCreator)
            : base(controlCreator, typeof(TestZekeView), "RBAC", MaterialIconKind.UserAccessControl, 2)
        {
        }

        public override Control CreateView(IControlCreator controlCreator, Type pageView) => controlCreator.CreateControl(pageView);
    }
}
