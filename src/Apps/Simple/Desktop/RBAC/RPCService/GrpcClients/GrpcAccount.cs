using Grpc.Core;
using Microsoft.Extensions.Configuration;
using Netx.Rbac.V1;
using NetX.AppCore.Contract.Extentions;
using Serilog;

namespace NetX.RBAC.RPCService
{
    public class GrpcAccount : RBACGrpcBase<AccountService.AccountServiceClient>, IAccountRPC
    {
        public GrpcAccount(IConfiguration configuration)
            : base(configuration)
        {
        }

        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <returns></returns>
        public async Task<CaptchaModel> GetCaptchaAsync()
        {
            try
            {
                var result = await base._client.GetCaptchaAsync(new Google.Protobuf.Empty());
                if(null == result || result.Status != Netx.Response.V1.Status.Ok)
                    throw new RpcException(new Status(StatusCode.Internal, $"code->{result?.Status},Error->{result?.Message}"));
                //data -> CaptchaModel
                return result.Data.ToModel<CaptchaModel>();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "获取验证码失败");
                return default;
            }
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="loginModel"></param>
        /// <returns></returns>
        public async Task<LoginResult> LoginAsync(LoginModel loginModel)
        {
            try
            {
                var result = await base._client.LoginAsync(new LoginRequest()
                {
                    UserName = loginModel.UserName,
                    Password = loginModel.Password,
                    CaptchaId = loginModel.CaptchaId,
                    Catpcha = loginModel.Captcha
                });
                if(null == result || result.Status != Netx.Response.V1.Status.Ok)
                    throw new Exception($"{result?.Message}");
                var resultModel = result.Data.ToModel<LoginResult>();
                resultModel.Success = true;
                Persional.Instance.JwtToken= resultModel.Token;
                return resultModel;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "LoginAsync Error");
                return new LoginResult() { Success = false , Message = ex.Message };
            }
        }

        public async Task LogoutAsync()
        {
            try
            {
                var result = await base._client.LogoutAsync(new Google.Protobuf.Empty());
                if (null == result || result.Status != Netx.Response.V1.Status.Ok)
                    throw new Exception($"{result?.Message}");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "LogoutAsync Error");
            }
        }

        protected override AccountService.AccountServiceClient CreateClient(CallInvoker call) => new AccountService.AccountServiceClient(call);
    }
}
