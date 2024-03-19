using DemoAddone.GrpcClients;
using DemoAddone.RPCService;
using Microsoft.Extensions.DependencyInjection;
using NetX.AppCore.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetX.RBAC
{
    public class DemoInitializer : IAddoneInitializer
    {
        public void ConfigureServices(IServiceCollection services)
        {
            GrpcRegister(services);
        }

        private void GrpcRegister(IServiceCollection services)
        {
            services.AddTransient<IDemo, GrpcDemo>();
        }
    }
}
