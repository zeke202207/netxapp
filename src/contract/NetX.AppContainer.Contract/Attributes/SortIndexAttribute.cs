using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetX.AppContainer.Contract
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class SortIndexAttribute : Attribute
    {
        public int Order { get; private set; }
        public bool Disabled { get; private set; }

        public SortIndexAttribute(int order, bool disabled =false)
        {
            Order = order;
            Disabled = disabled;
        }

        public void AddServices(ServiceCollection services, Type SelftType)
        {
            //startup singleton
            if (typeof(IStartupWindowViewModel).IsAssignableFrom(SelftType))
                services.AddSingleton(typeof(IStartupWindowViewModel), SelftType);
            else if(typeof(IMenuPageViewModel).IsAssignableFrom(SelftType))
                services.AddSingleton(typeof(IMenuPageViewModel), SelftType);
        }
    }
}
