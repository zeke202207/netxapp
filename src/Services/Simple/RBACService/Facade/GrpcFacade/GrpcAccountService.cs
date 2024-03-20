using AutoMapper;
using Google.Protobuf;
using Grpc.Core;
using Netx.Rbac.V1;
using Netx.Response.V1;
using NetX.RBAC.Service.Models;
using NetX.ServiceCore;

namespace NetX.RBAC.Service
{
    [GrpcEntry]
    public class GrpcAccountService : Netx.Rbac.V1.AccountService.AccountServiceBase
    {
        private readonly IServiceProvider _appServices;
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public GrpcAccountService(
            IMapper mapper,
            IServiceProvider appServices,
            IAccountService accountService)
        {
            _mapper = mapper;
            _appServices = appServices;
            _accountService = accountService;
        }

        public override async Task<Response> GetCaptcha(Empty request, ServerCallContext context)
        {
            var result = await context.ApplicationPipline<Empty, Response>(
                       request,
                       _appServices,
                       builder =>
                       {
                           builder.Use<AccountMiddleware<Empty, Response>>();
                       },
                        async r => _mapper.Map<CaptchaResultModel, Response>(await _accountService.GetCaptcha()));
            return Replay(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<Response> Login(LoginRequest request, ServerCallContext context)
        {
            var result = await context.ApplicationPipline<LoginRequest, Response>(
                       request,
                       _appServices,
                       builder =>
                       {
                           builder.Use<AccountMiddleware<LoginRequest, Response>>();
                       },
                        async r => _mapper.Map<LoginResultModel, Response>(await _accountService.Login(new Models.LoginModel()
                        {
                            UserName = request.UserName,
                            Password = request.Password,
                            Captcha = request.Catpcha,
                            CaptchaId = request.CaptchaId
                        })));
            return Replay(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<Response> Logout(Empty request, ServerCallContext context)
        {
            var result = await context.ApplicationPipline<Empty, Response>(
                       request,
                       _appServices,
                       builder =>
                       {
                           builder.Use<AuthMiddleware<Empty, Response>>();
                           builder.Use<AccountMiddleware<Empty, Response>>();
                       },
                        async r => new Response() { Status = Netx.Response.V1.Status.Ok });
            return Replay(result);
        }

        private Response Replay(GrpcResponse<Response> response)
        {
            if (response.IsSuccess)
                return response.Response;
            else
                return new Response()
                {
                    Status = Netx.Response.V1.Status.Error,
                    Message = response.Error.Message
                };
        }
    }
}
