using Avalonia.Controls;
using Avalonia.Controls.Templates;
using NetX.AppContainer.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MyDemo.ViewModels
{
    [StartStep(SplashScreenViewModel.Order)]
    [ViewModel(ServiceLifetime.Singleton)]
    public class SplashScreenViewModel : ViewModelBase
    {
        public const int Order = 0;
        private readonly IControlCreator _controlCreator;

        public SplashScreenViewModel(IControlCreator controlCreator) : base(SplashScreenViewModel.Order)
        {
            _controlCreator = controlCreator;
            Task.Run(() =>
            {
                System.Threading.Thread.Sleep(3000);
                AutoResetEvent.Set();
            });
        }

        protected override Control CreateView(string viewName)
        {
            return _controlCreator.CreateControl(Type.GetType(viewName));
        }
    }
}
