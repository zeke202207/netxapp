using MediatR;
using NetX.AppCore.Contract;
using SukiUI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetX.RBAC.EventBusHandlers
{
    [EventBusHander]
    public class UserInfoEventHandler : INotificationHandler<UserInfoEvent>
    {
        public Task Handle(UserInfoEvent notification, CancellationToken cancellationToken)
        {
            SukiHost.ShowToast($"User Info", $"click the user avantar, add your own logic");
            return Task.CompletedTask;
        }
    }
}
