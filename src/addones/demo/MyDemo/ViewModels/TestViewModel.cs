using Avalonia.Controls;
using NetX.AppContainer.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDemo.ViewModels
{
    [StartStep(TestViewModel.Order)]
    [ViewModel(ServiceLifetime.Singleton)]
    public class TestViewModel : ViewModelBase
    {
        public const int Order = 2;
        private readonly IControlCreator _controlCreator;

        public TestViewModel(IControlCreator controlCreator):base(TestViewModel.Order)
        {
            _controlCreator = controlCreator;
            Task.Run(() =>
            {
                System.Threading.Thread.Sleep(9000);
                AutoResetEvent.Set();
            });
        }

        protected override Control CreateView(string viewName)
        {
            return _controlCreator.CreateControl(Type.GetType(viewName));
        }
    }
}
