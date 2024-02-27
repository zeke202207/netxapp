using Avalonia.Controls;
using MyDemo.Views;
using NetX.AppContainer.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDemo.ViewModels
{
    [SortIndex(TestViewModel.Order)]
    [ViewModel(ServiceLifetime.Singleton)]
    public class TestViewModel : StartupWindowViewModel
    {
        public const int Order = 2;
        private readonly IControlCreator _controlCreator;

        public TestViewModel(IControlCreator controlCreator) 
            : base(controlCreator, typeof(TestWindow),TestViewModel.Order)
        {
            _controlCreator = controlCreator;
            Task.Run(() =>
            {
                System.Threading.Thread.Sleep(9000);
                AutoResetEvent.Set();
            });
        }

        public override Control CreateView(IControlCreator controlCreator, Type pageView) => controlCreator.CreateControl(pageView);
    }
}
