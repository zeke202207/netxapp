using NetX.ServiceCore;

namespace NetX.RBAC.Service
{
    public class AccountMiddleware<TRequest, TReponse> : IApplicationMiddleware<GrpcContext<TRequest, TReponse>>
    {
        private readonly IAccountService _accountService;

        public AccountMiddleware(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task InvokeAsync(ApplicationDelegate<GrpcContext<TRequest, TReponse>> next, GrpcContext<TRequest, TReponse> context)
        {
            context.Response.Response = await context.Handler?.Invoke((TRequest)context.Reqeust.Request);
            await next?.Invoke(context);
        }
    }
}
