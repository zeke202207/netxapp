using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetX.AppContainer.Contract;

public class UserInfoEvent : INotification
{
    public string Message { get; set; }

    public UserInfoEvent(string message)
    {
        Message = message;
    }
}
