using Avalonia.Controls;
using Avalonia.Controls.Templates;
using MyDemo.Views;
using NetX.AppContainer.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MyDemo.ViewModels
{
    [SortIndex(SplashScreenViewModel.Order)]
    [ViewModel(ServiceLifetime.Singleton)]
    public class SplashScreenViewModel : StartupWindowViewModel
    {
        public const int Order = 0;
        private readonly IControlCreator _controlCreator;

        public SplashScreenViewModel(IControlCreator controlCreator)
            : base(controlCreator, typeof(SplashScreenWindow), SplashScreenViewModel.Order)
        {
            _controlCreator = controlCreator;
            Task.Run(() =>
            {
                return;
                System.Threading.Thread.Sleep(3000);
                base.GotoNextWindow();
            });
        }

        public override Control CreateView(IControlCreator controlCreator, Type pageView) => controlCreator.CreateControl(pageView);

        protected override void ControlLoaded()
        {
            base.ControlLoaded();
        }
    }
}
