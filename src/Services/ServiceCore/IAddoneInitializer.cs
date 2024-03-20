﻿using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetX.ServiceCore
{
    public interface IAddoneInitializer
    {
        void ConfigureServices(IServiceCollection services);

        void ConfigureApp(IApplicationBuilder app);
    }
}