using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetX.AppCore.Contract;

/*
 未来扩展
 */
public class UserInfoEvent : INotification
{
    public InfoType InfoType { get; private set; }
    public dynamic ViewModel { get; private set; }

    public UserInfoEvent(InfoType infoType ,dynamic viewModel)
    {
        InfoType = infoType;
        ViewModel = viewModel;
    }
}

public enum InfoType
{
    ChangePassword,
    UserInfo,
    Relogin,
}
