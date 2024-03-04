using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NetX.AppContainer.Contract
{
    public interface IEventBus
    {
        Task Publish<TEvent>(TEvent @event) where TEvent : INotification;

        Task<TResponse> Send<TResponse>(IRequest<TResponse> request);

    }
}
