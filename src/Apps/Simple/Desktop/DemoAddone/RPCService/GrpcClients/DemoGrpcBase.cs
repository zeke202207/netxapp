using Grpc.Core.Interceptors;
using Microsoft.Extensions.Configuration;
using NetX.AppCore.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoAddone.GrpcClients
{
    public abstract class DemoGrpcBase<T> : BaseClient<T>
    {
        protected DemoGrpcBase(IConfiguration configuration)
            : base(configuration)
        {
            base._channel.Intercept();
        }
    }
}
