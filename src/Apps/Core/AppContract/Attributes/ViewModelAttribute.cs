using Microsoft.Extensions.DependencyInjection;

namespace NetX.AppCore.Contract
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
            var interfaces = SelftType;
            //startup singleton
            if (typeof(IStartupWindowViewModel).IsAssignableFrom(SelftType))
                interfaces = typeof(IStartupWindowViewModel);
            else if (typeof(IMenuPageViewModel).IsAssignableFrom(SelftType))
                interfaces = typeof(IMenuPageViewModel);
            else
                interfaces = SelftType;
            AddServices(services, SelftType, interfaces);
        }

        private void AddServices(ServiceCollection services, Type SelftType, Type InterfaceType)
        {
            _ = ServiceLifetime switch
            {
                ServiceLifetime.Singleton => services.AddSingleton(InterfaceType, SelftType),
                ServiceLifetime.Transient => services.AddTransient(InterfaceType, SelftType),
                ServiceLifetime.Scoped => services.AddScoped(InterfaceType, SelftType),
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
