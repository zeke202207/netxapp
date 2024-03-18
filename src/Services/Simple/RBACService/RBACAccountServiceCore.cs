using AutoMapper;
using Grpc.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using NetX.RBAC.Service.Models;
using NetX.ServiceCore;
using RBAC;

namespace NetX.RBAC.Service
{
    [GrpcEntry]
    public class RBACAccountServiceCore : RBACService.RBACServiceBase
    {
        private readonly IServiceProvider _appServices;
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public RBACAccountServiceCore(
            IMapper mapper,
            IServiceProvider appServices, 
            IAccountService accountService)
        {
            _mapper = mapper;
            _appServices = appServices;
            _accountService = accountService;
        }

        public override async Task<LoginResponse> Login(LoginRequest request, ServerCallContext context)
        {
            var result = await context.ApplicationPipline<LoginRequest, LoginResponse>(
                        request,
                        _appServices,
                        builder =>
                            {
                                builder.Use<AccountMiddleware<LoginRequest, LoginResponse>>();
                            },
                         async r => _mapper.Map<LoginResultModel, LoginResponse>(await _accountService.Login(new Models.LoginModel()
                         {
                             UserName = request.UserName,
                             Password = request.Password,
                             Captcha = ""
                         })));
            if (result.IsSuccess)
                return result.Response;
            else
                throw result.Error;
        }
    }
}
