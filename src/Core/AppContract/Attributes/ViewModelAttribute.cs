using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetX.AppContainer.Contract
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class ViewModelAttribute : Attribute
    {
        public ServiceLifetime ServiceLifetime { get; private set; } = ServiceLifetime.Transient;

        public ViewModelAttribute(ServiceLifetime serviceLifetime)
        {
            ServiceLifetime = serviceLifetime;
        }

        public void AddServices(ServiceCollection services, Type SelftType)
        {
            _ = ServiceLifetime switch
            {
                ServiceLifetime.Singleton => services.AddSingleton(SelftType, SelftType),
                ServiceLifetime.Transient => services.AddTransient(SelftType, SelftType),
                ServiceLifetime.Scoped => services.AddScoped(SelftType, SelftType),
                _ => throw new NotSupportedException()
            };
        }
    }

    public enum ServiceLifetime
    {
        Singleton,
        Transient,
        Scoped
    }
}
