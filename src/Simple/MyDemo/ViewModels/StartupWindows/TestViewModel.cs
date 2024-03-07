using Avalonia.Controls;
using MyDemo.Views;
using NetX.AppCore.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDemo.ViewModels
{
    [SortIndex(TestViewModel.Order, true)]
    [ViewModel(ServiceLifetime.Singleton)]
    public class TestViewModel : StartupWindowViewModel
    {
        public const int Order = 2;

        public TestViewModel(IControlCreator controlCreator) 
            : base(controlCreator, typeof(TestWindow),TestViewModel.Order)
        {
            Task.Run(() =>
            {
                System.Threading.Thread.Sleep(9000);
                GotoNextWindow();
            });
        }

        public override Control CreateView(IControlCreator controlCreator, Type pageView) => controlCreator.CreateControl(pageView);
    }
}
