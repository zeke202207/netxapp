using Microsoft.Extensions.DependencyInjection;
using NetX.AppContainer.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetX.AppContainer.Contract
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class StartStepAttribute : Attribute
    {
        public int Order { get; private set; }

        public StartStepAttribute(int order)
        {
            Order = order;
        }

        public void AddServices(ServiceCollection services, Type SelftType)
        {
            //startup singleton
            services.AddSingleton(typeof(IStartupViewModel),SelftType);
        }
    }
}
