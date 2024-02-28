using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NetX.AppContainer.Contract
{
    public class EventBus : IEventBus
    {
        private readonly IMediator _mediator;

        public EventBus(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Publish<TEvent>(TEvent @event) where TEvent : INotification
        {
            await _mediator.Publish(@event);
        }

        public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request)
        {
            return await _mediator.Send(request);
        }
    }
}
