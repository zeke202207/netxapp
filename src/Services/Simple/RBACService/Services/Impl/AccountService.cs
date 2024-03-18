using NetX.RBAC.Service.Models;
using NetX.ServiceCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetX.RBAC.Service
{
    [Transient]
    public class AccountService : IAccountService
    {
        public AccountService()
        { }

        public async Task<LoginResultModel> Login(LoginModel userModel)
        {
            await Task.Delay(2 * 1000);
            return new LoginResultModel
            {
                IsSuccess = true,
                Token = "abcdefg-123"
            };
        }
    }
}
