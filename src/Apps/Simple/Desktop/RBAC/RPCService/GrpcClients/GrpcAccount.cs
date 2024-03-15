using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using NetX.AppCore.Contract;
using RBAC;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetX.RBAC.RPCService
{
    public class GrpcAccount : RBACGrpcBase<RBACService.RBACServiceClient>, IAccountRPC
    {
        public GrpcAccount(IConfiguration configuration) 
            : base(configuration)
        {
        }

        public async Task<bool> Login(LoginModel loginModel)
        {
            try
            {
                var result = await base._client.LoginAsync(new LoginRequest()
                {
                    UserName = loginModel.UserName,
                    Password = loginModel.Password
                });
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Login Error");
                return false;
            }
        }

        protected override RBACService.RBACServiceClient CreateClient(CallInvoker call)
        {
            return new RBACService.RBACServiceClient(call);
        }
    }
}
