using Avalonia.Controls;
using Avalonia.Controls.Templates;
using MyDemo.Views;
using NetX.AppContainer.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDemo.ViewModels
{
    [SortIndex(LoginViewModel.Order, true)]
    [ViewModel(ServiceLifetime.Singleton)]
    public class LoginViewModel : StartupWindowViewModel
    {
        public const int Order = 1;
        private readonly IControlCreator _controlCreator;

        public LoginViewModel(IControlCreator controlCreator) : base(controlCreator, typeof(LoginWindow), LoginViewModel.Order)
        {
            _controlCreator = controlCreator;
            Task.Run(() =>
            {
                System.Threading.Thread.Sleep(6000);
                AutoResetEvent.Set();
            });
        }

        public override Control CreateView(IControlCreator controlCreator, Type pageView) => controlCreator.CreateControl(pageView);
    }
}
