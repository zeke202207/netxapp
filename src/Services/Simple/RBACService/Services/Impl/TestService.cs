using NetX.ServiceCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetX.RBAC.Service
{
    [Transient]
    public class TestService : ITestService
    {
        public bool Test()
        {
            return true;
        }
    }
}
