using Microsoft.Extensions.DependencyInjection;
using NetX.AppCore.Contract;
using NetX.RBAC.RPCService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetX.RBAC
{
    public class RBACInitializer : IAddoneInitializer
    {
        public void ConfigureServices(IServiceCollection services)
        {
            GrpcRegister(services);
        }

        private void GrpcRegister(IServiceCollection services)
        {
            services.AddTransient<IAccountRPC, GrpcAccount>();
        }
    }
}
