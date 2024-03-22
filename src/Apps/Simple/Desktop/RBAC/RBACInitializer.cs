using Microsoft.Extensions.DependencyInjection;
using NetX.AppCore.Contract;
using NetX.RBAC.RPCService;

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
