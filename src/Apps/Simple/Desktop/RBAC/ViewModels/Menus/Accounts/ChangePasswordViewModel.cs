using Avalonia.Controls;
using NetX.AppCore.Contract;
using NetX.RBAC.RPCService;
using NetX.RBAC.Views;
using ReactiveUI;
using SukiUI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace NetX.RBAC.ViewModels
{
    [ViewModel(ServiceLifetime.Transient)]
    public class ChangePasswordViewModel : BaseViewModel
    {

        private string _oldPassword;
        public string OldPassword
        {
            get => _oldPassword;
            set => this.RaiseAndSetIfChanged(ref _oldPassword, value);
        }

        private string _newPassword;
        public string NewPassword
        {
            get => _newPassword;
            set => this.RaiseAndSetIfChanged(ref _newPassword, value);
        }

        private string _newPasswordConfirm;
        public string NewPasswordConfirm
        {
            get => _newPasswordConfirm;
            set => this.RaiseAndSetIfChanged(ref _newPasswordConfirm, value);
        }

        public ReactiveCommand<Unit, Unit> ChangePasswordCommand { get; }
        public ReactiveCommand<Unit, Unit> CancelCommand { get; }

        private IAccountRPC _accountRPC;

        public ChangePasswordViewModel(IControlCreator controlCreator, IAccountRPC accountRPC) 
            : base(controlCreator, typeof(ChangePasswordView))
        {
            _accountRPC = accountRPC;
            ChangePasswordCommand = ReactiveCommand.Create(ChangePassword);
            CancelCommand = ReactiveCommand.Create(Cancel);
        }

        private void Cancel()
        {
            if(base.View is Window window)
                window.Close(false);
        }

        private async void ChangePassword()
        {            
            //TODO: 修改密码
            bool changeResult = true;

            SukiHost.ShowDialog(new DialogMessageViewModel(_controlCreator)
            {
                MessageType = DialogMessageType.Info,
                Message = "修改密码成功，请重新登录",
                Close = (result) =>
                {
                    if (result && base.View is Window window)
                        window.Close(changeResult);
                }
            });
        }

        public override Control CreateView(IControlCreator controlCreator, Type pageView)=> controlCreator.CreateControl(pageView);
    }
}
