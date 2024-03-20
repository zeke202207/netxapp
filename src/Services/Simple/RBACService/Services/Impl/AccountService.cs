using EasyCaching.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NetX.RBAC.Service.Common;
using NetX.RBAC.Service.Models;
using NetX.ServiceCore;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace NetX.RBAC.Service
{
    [Transient]
    public class AccountService : IAccountService
    {
        private readonly RBACDbContext _dbContext;
        private readonly ICaptcha _captcha;
        private readonly IEasyCachingProviderFactory _easyCachingProvider;
        private readonly IConfiguration _configuration;

        public AccountService(
            RBACDbContext dbContext,
            ICaptcha captcha, 
            IEasyCachingProviderFactory easyCachingProvider, 
            IConfiguration configuration)
        {
            _dbContext = dbContext;
            _captcha = captcha;
            _easyCachingProvider = easyCachingProvider;
            _configuration = configuration;
        }

        public async Task<CaptchaResultModel> GetCaptcha()
        {
            var captchaId = Guid.NewGuid().ToString();
            var captchaCode = await _captcha.GenerateRandomCaptchaAsync(4);
            var captcha = await _captcha.GenerateCaptchaImageAsync(captchaCode);
            // 缓存验证码，1分钟过期
            await _easyCachingProvider.GetCachingProvider("default").SetAsync(captchaId, captchaCode, TimeSpan.FromMinutes(1));
            return new CaptchaResultModel
            {
                CaptchaId = captchaId,
                CaptchaBase64 = Convert.ToBase64String(captcha)
            };
        }

        public async Task<LoginResultModel> Login(LoginModel userModel)
        {
            var captcha = await _easyCachingProvider.GetCachingProvider("default").GetAsync<string>(userModel.CaptchaId);
            if (!captcha.HasValue || !captcha.Value.Equals(userModel.Captcha,StringComparison.OrdinalIgnoreCase))
                throw new Exception("验证码错误");
            var entityUser = await _dbContext.sys_user.FirstOrDefaultAsync(p => p.UserName == userModel.UserName);
            var password = EncryptionHelper.CalculateSHA512(userModel.Password);
            if (entityUser == null || entityUser.Password != password)
                throw new Exception("用户名或密码错误");
            //生成jwt token
            var jwtToken = JwtHelper.GenerateJwtToken(
                _configuration["jwt:key"],
                _configuration["jwt:issuer"],
                _configuration["jwt:audience"],
                new Claim[]
                {
                    new Claim("USER_ID",$"{entityUser.Id}") ,
                    new Claim("USER_NAME",$"{entityUser.UserName}") ,
                    new Claim("USER_NICKNAME",$"{entityUser.NickName}")
                },
                DateTime.Now.AddMinutes(Convert.ToInt32(_configuration["jwt:expires"])));
            return new LoginResultModel
            {
                Token = jwtToken
            };
        }
    }
}
