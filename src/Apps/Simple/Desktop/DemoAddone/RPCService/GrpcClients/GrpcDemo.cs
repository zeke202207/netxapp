using DEMO;
using DemoAddone.RPCService;
using Grpc.Core;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoAddone.GrpcClients
{
    public class GrpcDemo : DemoGrpcBase<DEMO.DEMOService.DEMOServiceClient>, IDemo
    {
        public GrpcDemo(IConfiguration configuration) 
            : base(configuration)
        {
        }

        protected override DEMOService.DEMOServiceClient CreateClient(CallInvoker call)
        {
            return new DEMOService.DEMOServiceClient(call);
        }

        public async Task<bool> DemoCall()
        {
            try
            {
                var result = await base._client.TestAsync(new TestRequest()
                {
                    UserName = "zeke",
                    Password = "123321"
                });
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Login Error");
                return false;
            }
        }
    }
}
