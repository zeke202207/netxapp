using MediatR;

namespace NetX.AppCore.Contract
{
    public interface IEventBus
    {
        Task Publish<TEvent>(TEvent @event) where TEvent : INotification;

        Task<TResponse> Send<TResponse>(IRequest<TResponse> request);

    }
}
