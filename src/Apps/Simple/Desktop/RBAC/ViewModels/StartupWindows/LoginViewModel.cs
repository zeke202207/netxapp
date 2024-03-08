using Avalonia.Controls;
using NetX.AppCore.Contract;
using ReactiveUI;
using System.Reactive.Linq;
using System.Windows.Input;

namespace NetX.RBAC
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
