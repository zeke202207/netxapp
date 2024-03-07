using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Threading;
using MyDemo.Views;
using NetX.AppCore.Contract;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MyDemo.ViewModels
{
    [SortIndex(LoginViewModel.Order, false)]
    [ViewModel(ServiceLifetime.Singleton)]
    public class LoginViewModel : StartupWindowViewModel
    {
        public const int Order = 1;

        /// <summary>
        /// 
        /// </summary>
        public ICommand LoginCommand { get; }

        public LoginViewModel(IControlCreator controlCreator) 
            : base(controlCreator, typeof(LoginWindow), LoginViewModel.Order)
        {
            LoginCommand = ReactiveCommand.Create(() => Login(), CanExecute());
        }

        private IObservable<bool>? CanExecute()
        {
            return Observable.Return(true);
        }

        private void Login()
        {
            GotoNextWindow();
        }

        public override Control CreateView(IControlCreator controlCreator, Type pageView) => controlCreator.CreateControl(pageView);
    }
}
