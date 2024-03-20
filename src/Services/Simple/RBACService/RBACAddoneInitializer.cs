using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using NetX.ServiceCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetX.RBAC.Service
{
    public class RBACAddoneInitializer : IAddoneInitializer
    {
        public void ConfigureApp(IApplicationBuilder app, IConfiguration configuration)
        {
            
        }

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<TokenValidationParameters>(new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["jwt:key"])),

                ValidateIssuer = true,
                ValidIssuer = configuration["jwt:issuer"],
                
                ValidateAudience = true,
                ValidAudience = configuration["jwt:audience"],

                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            });

            services.AddEasyCaching(options =>
            {
                options.UseInMemory("default");
            });
        }
    }
}
