using Avalonia;
using Microsoft.Extensions.DependencyInjection;
using NetX.AppContainer.Common;
using NetX.AppContainer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetX.AppContainer.Extentions
{
    public static class ApplicationExtentions
    {
        public static IServiceProvider CreateServiceProvider(this Application app , Action<ServiceCollection> configServices)
        {
            var services = new ServiceCollection();
            var viewlocator = app.DataTemplates.FirstOrDefault(x => x is ViewLocator);
            if (viewlocator is not null)
                services.AddSingleton(viewlocator);
            configServices?.Invoke(services);
            return services.BuildServiceProvider();
        }
    }
}
