using NetX.RBAC.RPCService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetX.RBAC.RPCService
{
    public interface IAccountRPC
    {
        Task<CaptchaModel> GetCaptchaAsync();

        Task<LoginResult> LoginAsync(LoginModel loginModel);

        Task LogoutAsync();
    }
}
