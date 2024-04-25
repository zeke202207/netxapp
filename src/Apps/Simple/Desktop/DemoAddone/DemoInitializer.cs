using Avalonia.Controls;
using DemoAddone.Data;
using DemoAddone.GrpcClients;
using DemoAddone.RPCService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NetX.AppCore.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetX.RBAC
{
    public class DemoInitializer : IAddoneInitializer
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<BilibiliDataContext>(opt => opt.UseSqlite(@"Data Source=Storage\database\bilibili.db"));
            GrpcRegister(services);
        }

        private void GrpcRegister(IServiceCollection services)
        {
            services.AddTransient<IDemo, GrpcDemo>();
            services.AddTransient<IFileExplorerManager, LocalFileExplorerManager>();
        }
    }
}
