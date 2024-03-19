using Microsoft.EntityFrameworkCore;
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
        private readonly RBACDbContext _dbContext;

        public AccountService(RBACDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<LoginResultModel> Login(LoginModel userModel)
        {
            var entityUser = await _dbContext.sys_user.FirstOrDefaultAsync(p=>p.UserName == userModel.UserName);
            await Task.Delay(1 * 1000);
            return new LoginResultModel
            {
                IsSuccess = true,
                Token = "abcdefg-123"
            };
        }
    }
}
