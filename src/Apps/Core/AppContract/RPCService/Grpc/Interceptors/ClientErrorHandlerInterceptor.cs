using Grpc.Core;
using Grpc.Core.Interceptors;
using Serilog;

namespace NetX.AppCore.Contract.RPCService
{
    public class ClientErrorHandlerInterceptor : Interceptor
    {
        // Unary Call
        public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(
            TRequest request,
            ClientInterceptorContext<TRequest, TResponse> context,
            AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
        {
            AsyncUnaryCall<TResponse> result = continuation(request, context);
            return new AsyncUnaryCall<TResponse>(
                HandleResponseAsync(result.ResponseAsync),
                result.ResponseHeadersAsync,
                result.GetStatus,
                result.GetTrailers,
                result.Dispose);
        }

        // Client Streaming Call
        public override AsyncClientStreamingCall<TRequest, TResponse> AsyncClientStreamingCall<TRequest, TResponse>(
            ClientInterceptorContext<TRequest, TResponse> context,
            AsyncClientStreamingCallContinuation<TRequest, TResponse> continuation)
        {
            AsyncClientStreamingCall<TRequest, TResponse> result = continuation(context);
            return new AsyncClientStreamingCall<TRequest, TResponse>(
                result.RequestStream,
                HandleResponseAsync(result.ResponseAsync),
                result.ResponseHeadersAsync,
                result.GetStatus,
                result.GetTrailers,
                result.Dispose);
        }

        // Server Streaming Call
        public override AsyncServerStreamingCall<TResponse> AsyncServerStreamingCall<TRequest, TResponse>(
            TRequest request,
            ClientInterceptorContext<TRequest, TResponse> context,
            AsyncServerStreamingCallContinuation<TRequest, TResponse> continuation)
        {
            AsyncServerStreamingCall<TResponse> result = continuation(request, context);
            return new AsyncServerStreamingCall<TResponse>(
                new ExceptionHandlingAsyncStreamReader<TResponse>(result.ResponseStream),
                result.ResponseHeadersAsync,
                result.GetStatus,
                result.GetTrailers,
                result.Dispose);
        }

        // Duplex Streaming Call
        public override AsyncDuplexStreamingCall<TRequest, TResponse> AsyncDuplexStreamingCall<TRequest, TResponse>(
            ClientInterceptorContext<TRequest, TResponse> context,
            AsyncDuplexStreamingCallContinuation<TRequest, TResponse> continuation)
        {
            AsyncDuplexStreamingCall<TRequest, TResponse> result = continuation(context);
            return new AsyncDuplexStreamingCall<TRequest, TResponse>(
                result.RequestStream,
                new ExceptionHandlingAsyncStreamReader<TResponse>(result.ResponseStream),
                result.ResponseHeadersAsync,
                result.GetStatus,
                result.GetTrailers,
                result.Dispose);
        }

        private async Task<TResponse> HandleResponseAsync<TResponse>(Task<TResponse> task)
        {
            try
            {
                return await task;
            }
            catch (Exception ex)
            {
                Log.Error($"An error occurred: {ex}");
                throw ex;
            }
        }

        // 用于处理Server Streaming和Duplex Streaming的响应流
        private class ExceptionHandlingAsyncStreamReader<T> : IAsyncStreamReader<T>
        {
            private readonly IAsyncStreamReader<T> _inner;

            public ExceptionHandlingAsyncStreamReader(IAsyncStreamReader<T> inner)
            {
                _inner = inner;
            }

            public T Current => _inner.Current;

            public async Task<bool> MoveNext(CancellationToken cancellationToken)
            {
                try
                {
                    return await _inner.MoveNext(cancellationToken).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    Log.Error($"An error occurred while streaming response: {ex}");
                    throw ex;
                }
            }
        }
    }
}
