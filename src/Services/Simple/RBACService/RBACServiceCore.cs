using Grpc.Core;
using NetX.ServiceCore;
using RBAC;

namespace NetX.RBAC.Service
{
    [GrpcEntry]
    public class RBACServiceCore : RBACService.RBACServiceBase
    {
        private readonly IServiceProvider _appServices;

        public RBACServiceCore(IServiceProvider appServices)
        {
            _appServices = appServices;
        }

        public override async Task<LoginResponse> Login(LoginRequest request, ServerCallContext context)
        {
            var result = await context.ApplicationPipline<LoginRequest, LoginResponse>(request, _appServices, builder =>
            {
                builder.Use<AuthMiddleware<LoginRequest, LoginResponse>>();
            });
            if (result.IsSuccess)
                return result.Response;
            else
                throw result.Error;
        }
    }
}
