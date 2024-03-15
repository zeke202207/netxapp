using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetX.RBAC.RPCService
{
    public interface IAccountRPC
    {
        Task<bool> Login(LoginModel loginModel);
    }
}
