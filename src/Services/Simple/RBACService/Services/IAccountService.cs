using NetX.RBAC.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetX.RBAC.Service
{
    public interface IAccountService
    {
        Task<CaptchaResultModel> GetCaptcha();

        Task<LoginResultModel> Login(LoginModel userModel);
    }
}
