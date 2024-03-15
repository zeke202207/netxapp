using Grpc.Core.Interceptors;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using NetX.AppCore.Contract;
using NetX.AppCore.Contract.RPCService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetX.RBAC.RPCService
{
    public abstract class RBACGrpcBase<T> : BaseClient<T>
    {
        protected RBACGrpcBase(IConfiguration configuration) 
            : base(configuration)
        {
            base._channel.Intercept();
        }
    }
}
