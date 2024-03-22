using Grpc.Core;
using Grpc.Core.Interceptors;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using NetX.AppCore.Contract.RPCService;
namespace NetX.AppCore.Contract
{

    public abstract class BaseClient<T> : IDisposable
    {
        private readonly IConfiguration _configuration;
        private readonly string _address;
        protected GrpcChannel _channel;
        protected T _client;

        public BaseClient(IConfiguration configuration)
        {
            this._configuration = configuration;
            this._address = _configuration.GetSection("rpc").GetSection("address").Value;
            InitializeClient();
        }

        protected virtual void InitializeClient()
        {
            _channel = GrpcChannel.ForAddress(_address, new GrpcChannelOptions()
            {
                MaxSendMessageSize = int.MaxValue,
                MaxReceiveMessageSize = int.MaxValue,
            });
            var interceptors = CustomerInterceptors().ToList();
            interceptors.Insert(0, new ClientErrorHandlerInterceptor());
            interceptors.Insert(1, new ClientLoggerHandlerInterceptor());
            var callInvoker = _channel.CreateCallInvoker().Intercept(interceptors.ToArray());
            _client = CreateClient(callInvoker);
        }

        /// <summary>
        /// 创建grpc客户端
        /// </summary>
        /// <param name="channel"></param>
        /// <returns></returns>
        protected abstract T CreateClient(CallInvoker call);

        protected virtual IEnumerable<Interceptor> CustomerInterceptors()
        {
            return Array.Empty<Interceptor>();
        }

        public void Dispose()
        {
            _channel?.ShutdownAsync().Wait();
            _channel?.Dispose();
        }
    }
}
