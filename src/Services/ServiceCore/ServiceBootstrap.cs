using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace NetX.ServiceCore
{
    public class ServiceBootstrap
    {
        public static async Task Start(string[] args)
        {
            await Start<Startup>(args);
        }   

        public static async Task Start<T>(string[] args)
            where T : Startup
        {
            var hostBuilder = Host.CreateDefaultBuilder(args)
               .ConfigureAppConfiguration((hostingContext, config) =>
               {
                   DirectoryInfo directory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
                   foreach(var file in directory.GetFiles("appsettings.*.json"))
                   {
                       config.AddJsonFile(file.FullName);
                   }
                   //config.AddJsonFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json"));
                   //config.AddJsonFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.addone.json"));
                   //config.AddJsonFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.log.json"));
                   //config.AddJsonFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.service.json"));
               })
               .ConfigureWebHostDefaults(webHostBuilder =>
               {
                   webHostBuilder.ConfigureKestrel(options =>
                   {
                       var configuration = new ConfigurationBuilder()
                                       .AddJsonFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.service.json"), optional: true, reloadOnChange: true)
                                       .Build();
                       options.ListenAnyIP(configuration.GetValue<int>("allowedhosts:httpport"), listenOptions => listenOptions.Protocols = HttpProtocols.Http1);
                       options.ListenAnyIP(configuration.GetValue<int>("allowedhosts:http2port"), listenOptions => listenOptions.Protocols = HttpProtocols.Http2);
                   });
                   webHostBuilder.UseStartup<T>();
               });
            var host = hostBuilder.Build();
            await host.RunAsync();
        }
    }
}
