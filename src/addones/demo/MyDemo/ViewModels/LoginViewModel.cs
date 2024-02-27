using Avalonia.Controls;
using Avalonia.Controls.Templates;
using NetX.AppContainer.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDemo.ViewModels
{
    [StartStep(LoginViewModel.Order)]
    [ViewModel(ServiceLifetime.Singleton)]
    public class LoginViewModel : ViewModelBase
    {
        public const int Order = 1;
        private readonly IControlCreator _controlCreator;

        public LoginViewModel(IControlCreator controlCreator) : base(LoginViewModel.Order)
        {
            _controlCreator = controlCreator;
            Task.Run(() =>
            {
                System.Threading.Thread.Sleep(6000);
                AutoResetEvent.Set();
            });
        }

        protected override Control CreateView(string viewName)
        {
            return _controlCreator.CreateControl(Type.GetType(viewName));
        }
    }
}
