using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetX.AppCore.Contract
{
    public interface IAddoneInitializer
    {
        void ConfigureServices(IServiceCollection services);
    }
}
