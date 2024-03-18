using NetX.ServiceCore;

namespace NetX.RBAC.Service;

public class AuthMiddleware<TRequest, TReponse> : IApplicationMiddleware<GrpcContext<TRequest, TReponse>>
{
    private readonly IAccountService _testService;

    public AuthMiddleware(IAccountService testService)
    {
        _testService = testService;
    }

    public async Task InvokeAsync(ApplicationDelegate<GrpcContext<TRequest, TReponse>> next, GrpcContext<TRequest, TReponse> context)
    {
        var canAccess = true;
        if (!canAccess)
        {
            context.Response.Error = new UnauthorizedAccessException("未授权");
            throw context.Response.Error;
        }
        await next?.Invoke(context);
    }
}
