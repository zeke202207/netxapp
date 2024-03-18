using Grpc.Core;
using Microsoft.AspNetCore.Identity.Data;

namespace NetX.ServiceCore
{

    public static class GrpcExtensions
    {
        /// <summary>
        /// Grpc Context扩展方法
        /// 构建自定义管道上下文
        /// </summary>
        /// <typeparam name="TRequest"></typeparam>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="context"></param>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        internal static GrpcContext<TRequest, TResponse> CreateGrpcContext<TRequest, TResponse>(this ServerCallContext context, TRequest request, TResponse response)
        {
            var client = new GrpcClient(context);
            var grpcRequest = GrpcRequest<TRequest>.Create(request);
            var grpcResponse = new GrpcResponse<TResponse>(response);
            var grpcContext = new GrpcContext<TRequest, TResponse>(client, grpcRequest, grpcResponse, context.GetHttpContext().Features)
            {
                CancellationToken = context.CancellationToken
            };
            return grpcContext;
        }

        public static async Task<GrpcResponse<TResponse>> ApplicationPipline<TRequest, TResponse>(
            this ServerCallContext context,
            TRequest request,
            IServiceProvider _appServices,
            Action<ApplicationBuilder<GrpcContext<TRequest, TResponse>>> useMiddleware,
            Func<TRequest, Task<TResponse>> @hander
            )
            where TRequest : class
            where TResponse : class, new()
        {
            var grpcContext = context.CreateGrpcContext<TRequest, TResponse>(request, new TResponse());
            grpcContext.Handler = @hander;
            try
            {
                var appBuilder = new ApplicationBuilder<GrpcContext<TRequest, TResponse>>(_appServices)
                    .Use<ExceptionMiddleware<TRequest, TResponse>>();
                useMiddleware?.Invoke(appBuilder);
                var application = appBuilder.Build();
                await application.Invoke(grpcContext);
                grpcContext.Response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                grpcContext.Response.IsSuccess = false;
                grpcContext.Response.Error = ex;
            }
            return grpcContext.Response;
        }
    }
}
