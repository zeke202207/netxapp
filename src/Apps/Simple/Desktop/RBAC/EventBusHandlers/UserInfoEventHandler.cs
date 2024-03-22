using Avalonia.Controls;
using MediatR;
using NetX.AppCore.Contract;
using NetX.RBAC.ViewModels;
using NetX.RBAC.Views;

namespace NetX.RBAC.EventBusHandlers
{
    [EventBusHander]
    public class UserInfoEventHandler : INotificationHandler<UserInfoEvent>
    {
        private IControlCreator _controlCreator;
        private readonly ChangePasswordViewModel _changePasswordViewModel;

        public UserInfoEventHandler(
            IControlCreator controlCreator, ChangePasswordViewModel changePasswordViewModel)
        {
            _controlCreator = controlCreator;
            _changePasswordViewModel = changePasswordViewModel;
        }

        public async Task Handle(UserInfoEvent notification, CancellationToken cancellationToken)
        {
            switch (notification.InfoType)
            {
                case InfoType.UserInfo:
                    ShowUserInfo(notification);
                    break;
                case InfoType.ChangePassword:
                    await ShowChangePassword(notification);
                    break;
                case InfoType.Relogin:
                    ShowRelogin(notification);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 重新返回登录页面
        /// </summary>
        /// <param name="notification"></param>
        private void ShowRelogin(UserInfoEvent notification)
        {
            notification.ViewModel.GotoStartupWindow(LoginViewModel.Order);
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="notification"></param>
        /// <returns></returns>
        private async Task ShowChangePassword(UserInfoEvent notification)
        {
            var view = _changePasswordViewModel.CreateView(typeof(ChangePasswordView));
            if (view is Window window && await window.ShowDialog<bool>(notification.ViewModel.Window))
                ShowRelogin(notification);
        }

        /// <summary>
        /// 修改用户信息
        /// 注：修改成功后刷新界面
        /// </summary>
        /// <param name="notification"></param>
        private void ShowUserInfo(UserInfoEvent notification)
        {
            //TODO: 更新用户信息，成功后刷新界面
            notification.ViewModel.UserName = "admin";
        }
    }
}
