using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;

namespace NetX.ServiceCore;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServiceCore(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ServiceAddoneConfig>(configuration)
            .AddOptions<ServiceAddoneConfig>();

        services.AddHttpContextAccessor();
        services.AddControllers();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["jwt:issuer"],
                    ValidAudience = configuration["jwt:audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["jwt:key"]))
                };
            });

        services.AddGrpc(options =>
        {
            options.Interceptors.Add<GrpcConnectionInterceptor>();
            options.MaxSendMessageSize = int.MaxValue;
            options.MaxReceiveMessageSize = int.MaxValue;
            options.EnableDetailedErrors = true;
            //根据业务需求，可以选择开启grpc压缩（未来可配置到appsettings.json配置文件中）
            //options.ResponseCompressionAlgorithm = "gzip";
            //options.ResponseCompressionLevel = System.IO.Compression.CompressionLevel.Fastest;
            //options.CompressionProviders.Add(new GzipCompressionProvider(System.IO.Compression.CompressionLevel.Fastest));
        });

        services.AddServiceAddone(configuration);
        services.AddServiceAddoneInitializer(configuration);
        services.AddAutoMapper(configuration);
        services.AddDbContexts(configuration);

        return services;
    }

    private static IServiceCollection AddServiceAddone(this IServiceCollection services, IConfiguration configuration)
    {
        var addones = configuration.GetSection("addoneassembly")
            .Get<string[]>();

        foreach (var addone in addones)
        {
            var assembly = Assembly.Load(addone);
            services.AddServicesFromAssembly(assembly);
        }

        return services;
    }

    /// <summary>
    /// 从指定程序集中注入服务
    /// </summary>
    /// <param name="services"></param>
    /// <param name="assembly"></param>
    /// <returns></returns>
    private static IServiceCollection AddServicesFromAssembly(this IServiceCollection services, Assembly assembly)
    {
        foreach (var type in assembly.GetTypes())
        {
            #region ==单例注入==

            var singletonAttr = (SingletonAttribute)Attribute.GetCustomAttribute(type, typeof(SingletonAttribute));
            if (singletonAttr != null)
            {
                //注入自身类型
                if (singletonAttr.Itself)
                {
                    services.AddSingleton(type);
                    continue;
                }

                var interfaces = type.GetInterfaces().Where(m => m != typeof(IDisposable)).ToList();
                if (interfaces.Any())
                {
                    foreach (var i in interfaces)
                    {
                        services.AddSingleton(i, type);
                    }
                }
                else
                {
                    services.AddSingleton(type);
                }

                continue;
            }

            #endregion

            #region ==瞬时注入==

            var transientAttr = (TransientAttribute)Attribute.GetCustomAttribute(type, typeof(TransientAttribute));
            if (transientAttr != null)
            {
                //注入自身类型
                if (transientAttr.Itself)
                {
                    services.AddSingleton(type);
                    continue;
                }

                var interfaces = type.GetInterfaces().Where(m => m != typeof(IDisposable)).ToList();
                if (interfaces.Any())
                {
                    foreach (var i in interfaces)
                    {
                        services.AddTransient(i, type);
                    }
                }
                else
                {
                    services.AddTransient(type);
                }
                continue;
            }

            #endregion

            #region ==Scoped注入==
            var scopedAttr = (ScopedAttribute)Attribute.GetCustomAttribute(type, typeof(ScopedAttribute));
            if (scopedAttr != null)
            {
                //注入自身类型
                if (scopedAttr.Itself)
                {
                    services.AddSingleton(type);
                    continue;
                }

                var interfaces = type.GetInterfaces().Where(m => m != typeof(IDisposable)).ToList();
                if (interfaces.Any())
                {
                    foreach (var i in interfaces)
                    {
                        services.AddScoped(i, type);
                    }
                }
                else
                {
                    services.AddScoped(type);
                }
            }

            #endregion
        }

        return services;
    }

    /// <summary>
    /// 注册AddoneInitializer
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    private static IServiceCollection AddServiceAddoneInitializer(this IServiceCollection services, IConfiguration configuration)
    {
        var addones = configuration.GetSection("addoneassembly")
            .Get<string[]>();
        foreach (var addone in addones)
        {
            var assembly = Assembly.Load(addone);
            var types = assembly.GetTypes().Where(type => type.GetInterfaces().Contains(typeof(IAddoneInitializer)));
            foreach (var type in types)
            {
                var initializer = (IAddoneInitializer)Activator.CreateInstance(type);
                initializer.ConfigureServices(services);
            }
        }

        return services;
    }

    /// <summary>
    /// 注册AutoMapper
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    private static IServiceCollection AddAutoMapper(this IServiceCollection services, IConfiguration configuration)
    {
        var config = new MapperConfiguration(cfg =>
        {
            var addones = configuration.GetSection("addoneassembly")
            .Get<string[]>();

            foreach (var addone in addones)
            {
                var assembly = Assembly.Load(addone);
                var types = assembly.GetTypes().Where(type => type.GetInterfaces().Contains(typeof(IMapperConfig)));
                foreach (var type in types)
                {
                    var initializer = (IMapperConfig)Activator.CreateInstance(type);
                    initializer.Bind(cfg);
                }
            }
        });
        services.AddSingleton(config.CreateMapper());

        return services;
    }

    /// <summary>
    /// 注册DbContext
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    private static IServiceCollection AddDbContexts(this IServiceCollection services, IConfiguration configuration)
    {
        var addones = configuration.GetSection("addoneassembly")
           .Get<string[]>();
        foreach (var addone in addones)
        {
            var assembly = Assembly.Load(addone);
            var dbContextTypes = assembly.GetTypes().Where(type => type.IsClass && !type.IsAbstract && type.IsSubclassOf(typeof(DbContext)));
            foreach (var dbContextType in dbContextTypes)
            {
                var connectionString = configuration["connections:connstr"];
                if (!string.IsNullOrEmpty(connectionString))
                {
                    // 更精确地寻找AddDbContext方法
                    var addDbContextMethod = typeof(EntityFrameworkServiceCollectionExtensions)
                        .GetMethods(BindingFlags.Public | BindingFlags.Static)
                        .FirstOrDefault(method => method.Name == "AddDbContext" &&
                                                  method.GetGenericArguments().Length == 1 && // 确保是泛型方法
                                                  method.GetParameters().Any(p => p.ParameterType == typeof(Action<DbContextOptionsBuilder>))); // 检查是否接受Action<DbContextOptionsBuilder>

                    if (addDbContextMethod != null)
                    {
                        var genericMethod = addDbContextMethod.MakeGenericMethod(dbContextType);
                        genericMethod.Invoke(null, 
                            new object[] 
                            { 
                                services, 
                                new Action<DbContextOptionsBuilder>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))),
                                ServiceLifetime.Scoped,
                                ServiceLifetime.Scoped
                            });
                    }
                }
            }
        }
        return services;
    }
}
