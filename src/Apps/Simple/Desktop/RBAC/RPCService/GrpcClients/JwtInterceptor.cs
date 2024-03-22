using Grpc.Core;
using Grpc.Core.Interceptors;

namespace NetX.RBAC.RPCService.GrpcClients
{
    public class JwtInterceptor : Interceptor
    {
        private readonly string jwtToken;

        public JwtInterceptor(string jwtToken)
        {
            this.jwtToken = jwtToken;
        }

        // Unary Call
        public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(
            TRequest request,
            ClientInterceptorContext<TRequest, TResponse> context,
            AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
        {
            AddHeader(context.Options.Headers);
            return base.AsyncUnaryCall(request, context, continuation);
        }

        // Client Streaming Call
        public override AsyncClientStreamingCall<TRequest, TResponse> AsyncClientStreamingCall<TRequest, TResponse>(
            ClientInterceptorContext<TRequest, TResponse> context,
            AsyncClientStreamingCallContinuation<TRequest, TResponse> continuation)
        {
            AddHeader(context.Options.Headers);
            return base.AsyncClientStreamingCall(context, continuation);
        }

        // Server Streaming Call
        public override AsyncServerStreamingCall<TResponse> AsyncServerStreamingCall<TRequest, TResponse>(
            TRequest request,
            ClientInterceptorContext<TRequest, TResponse> context,
            AsyncServerStreamingCallContinuation<TRequest, TResponse> continuation)
        {
            AddHeader(context.Options.Headers);
            return base.AsyncServerStreamingCall(request, context, continuation);
        }

        // Duplex Streaming Call
        public override AsyncDuplexStreamingCall<TRequest, TResponse> AsyncDuplexStreamingCall<TRequest, TResponse>(
            ClientInterceptorContext<TRequest, TResponse> context,
            AsyncDuplexStreamingCallContinuation<TRequest, TResponse> continuation)
        {
            AddHeader(context.Options.Headers);
            return base.AsyncDuplexStreamingCall(context, continuation);
        }

        private void AddHeader(Metadata headers)
        {
            if (string.IsNullOrEmpty(jwtToken))
                return;
            if (null == headers)
                headers = new Metadata();
            headers.Add("Authorization", $"Bearer {jwtToken}");
        }
    }
}
