﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoAddone.RPCService
{
    public interface IDemo
    {
        Task<bool> DemoCall();
    }
}
