using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace NetX.ServiceCore;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseServiceCore(
        this IApplicationBuilder app, 
        IConfiguration configuration)
    {
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseGrpcEntry(configuration);

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        return app;
    }

    private static IApplicationBuilder UseGrpcEntry(this IApplicationBuilder app, IConfiguration configuration)
    {
        var addones = configuration.GetSection("addoneassembly")
          .Get<string[]>();
        foreach(var addone in addones)
        {
            var assembly = Assembly.Load(addone);
            var types = assembly.GetTypes().Where(type => type.GetCustomAttributes(typeof(GrpcEntryAttribute), true).Any());
            foreach (var type in types)
            {
                app.UseEndpoints(endpoints =>
                {
                    // 使用反射找到MapGrpcService的泛型方法，并为当前type构造一个方法实例
                    var method = typeof(GrpcEndpointRouteBuilderExtensions)
                        .GetMethod(nameof(GrpcEndpointRouteBuilderExtensions.MapGrpcService), 1, new Type[] { typeof(IEndpointRouteBuilder) })
                        ?.MakeGenericMethod(type);
                    // 调用构造出的泛型方法
                    method?.Invoke(null, new object[] { endpoints });
                });
            }
        }

        return app;
    }
}
