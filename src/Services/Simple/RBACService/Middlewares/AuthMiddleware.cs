using NetX.ServiceCore;

namespace NetX.RBAC.Service;

public class AuthMiddleware<TRequest, TReponse> : IApplicationMiddleware<GrpcContext<TRequest, TReponse>>
{
    private readonly ITestService _testService;

    public AuthMiddleware(ITestService testService)
    {
        _testService = testService;
    }

    public async Task InvokeAsync(ApplicationDelegate<GrpcContext<TRequest, TReponse>> next, GrpcContext<TRequest, TReponse> context)
    {
        var canAccess = true;
        if (!canAccess)
            throw new UnauthorizedAccessException("未授权");
        await next?.Invoke(context);
    }
}
