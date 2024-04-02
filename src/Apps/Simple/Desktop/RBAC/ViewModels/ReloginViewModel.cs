using FluentAvalonia.UI.Controls;
using NetX.AppCore.Contract;
using NetX.AppCore.Contract.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetX.RBAC.ViewModels
{
    [ViewModel(ServiceLifetime.Singleton)]
    public class ReloginViewModel : FlyoutMenuViewModel
    {
        public ReloginViewModel()
        {
            Title = "重新登录";
            IconSource = Symbol.Redo;
        }

        protected override void Excute()
        {
            base.StartupWindow.GotoWindow(LoginViewModel.Id);
        }
    }
}
