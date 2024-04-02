using Avalonia.Controls;
using FluentAvalonia.UI.Controls;
using NetX.AppCore.Contract;
using NetX.AppCore.Contract.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetX.AppCore.ViewModels
{
    [ViewModel(ServiceLifetime.Singleton)]
    public class ExitAppViewModel : FlyoutMenuViewModel
    {
        public ExitAppViewModel()
        {
            Title = "退出";
            IconSource = Symbol.SignOut;
        }

        protected override void Excute()
        {
            //TODO: 退出应用程序
            base.StartupWindow.GotoWindow(Guid.Empty);
        }
    }
}
