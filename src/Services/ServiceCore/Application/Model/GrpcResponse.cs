using Grpc.Core;

namespace NetX.ServiceCore;

public sealed class GrpcResponse<T>
{
    public Metadata MetaData { get; set; }

    public T Response { get; set; }

    public bool IsSuccess { get; set; }

    public Exception Error { get; set; }

    public IServerStreamWriter<T> ResponseStream { get; set; }

    public GrpcResponse(T response)
    {
        MetaData = new Metadata();
        Response = response;
    }
}
