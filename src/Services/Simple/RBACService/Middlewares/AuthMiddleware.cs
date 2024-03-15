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
        var canAccess = false;
        if (!canAccess)
        {
            Thread.Sleep(2 * 1000);
            context.Response.Error = new UnauthorizedAccessException("未授权");
            throw context.Response.Error;
        }
        await next?.Invoke(context);
    }
}
