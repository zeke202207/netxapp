﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetX.RBAC.RPCService
{
    public class LoginModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Code { get; set; }
    }
}
