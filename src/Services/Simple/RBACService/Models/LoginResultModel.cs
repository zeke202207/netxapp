using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetX.RBAC.Service.Models
{
    public class LoginResultModel
    {
        public bool IsSuccess { get; set; }
        public string Token { get; set; }
    }
}
