using Avalonia.Controls;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;

namespace NetX.AppCore.Contract
{
    public class ActivatorControlCreator : IControlCreator
    {
        private IServiceProvider _serviceProvider;

        public ActivatorControlCreator(IServiceProvider sp)
        {
            _serviceProvider = sp;
        }

        public Control? CreateControl(Type controlType, bool keepalive = false)
        {
            return (Control)ActivatorUtilities.CreateInstance(_serviceProvider, controlType);
        }
    }
}
