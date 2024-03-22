using Grpc.Core.Interceptors;
using Microsoft.Extensions.Configuration;
using NetX.AppCore.Contract;
using NetX.RBAC.RPCService.GrpcClients;

namespace NetX.RBAC.RPCService
{
    public abstract class RBACGrpcBase<T> : BaseClient<T>
    {
        protected RBACGrpcBase(IConfiguration configuration)
            : base(configuration)
        {
            base._channel.Intercept();
        }

        protected override IEnumerable<Interceptor> CustomerInterceptors()
        {
            return new List<Interceptor>()
            {
                new JwtInterceptor(Persional.Instance.JwtToken)
            };
        }
    }
}
