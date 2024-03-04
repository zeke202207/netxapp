using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Templates;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetX.AppContainer.Contract;
using NetX.AppContainer.Extentions;
using NetX.AppContainer.Models;
using NetX.AppContainer.ViewModels;
using NetX.AppContainer.Views;
using SukiUI;
using SukiUI.Controls;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace NetX.AppContainer;

public partial class App : Application
{
    private readonly ServiceCollection _services;
    private ServiceProvider _serviceProvider;
    private IConfiguration _configuration;

    public App()
    {
        _services = new ServiceCollection();
    }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
        ConfigureJsonConfig();
        ConfigureServices(_services);
        ConfigAddOneServices(_services);
        _serviceProvider = _services.BuildServiceProvider();
    }

    private void ConfigureJsonConfig()
    {
        var builder = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile($"{Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsetting.json")} ");
        _configuration = builder.Build();
    }

    private void ConfigureServices(ServiceCollection services)
    {
        // add gloable config
        services.Configure<AppConfig>(_configuration)
            .AddOptions<AppConfig>();

        // event bus
        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssemblies(new[] { Assembly.GetExecutingAssembly(), typeof(App).Assembly });
        });
        services.AddSingleton<IEventBus, EventBus>();

        // viewlocator
        var viewlocator = Current?.DataTemplates.First(x => x is ViewLocator);
        if (viewlocator is not null)
            services.AddSingleton(viewlocator);

        services.AddSingleton<IControlCreator, ActivatorControlCreator>();
        services.AddSingleton<AppBootstrap>();
        //viewmodel
        services.AddSingleton<CustomThemeDialogViewModel>();
        services.AddSingleton<SukiTheme>();
        services.AddSingleton<MainViewModel>();
        services.AddSingleton<IStartupWindowViewModel,MainViewModel>();
    }

    private void ConfigAddOneServices(ServiceCollection services)
    {
        ConfigStartStepServices(services);
        ConfigViewModelServices(services);
        ConfigEventBusServices(services);
    }

    private void ConfigEventBusServices(ServiceCollection services)
    {
        var types = Assembly.GetEntryAssembly()!.GetTypes();
        foreach (var type in types)
        {
            foreach (var interfaceType in type.GetInterfaces())
            {
                if (interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == typeof(INotificationHandler<>))
                    services.AddSingleton(interfaceType, type);
            }
        }
    }

    private void ConfigStartStepServices(ServiceCollection services)
    {
        Assembly.GetEntryAssembly()!.GetTypes()
           .Where(type => null != type.GetCustomAttribute<SortIndexAttribute>())
           .OrderBy(type => type.GetCustomAttribute<SortIndexAttribute>()!.Order)
           .ToList().ForEach(addOneType =>
           {
               if (!CanInjection(addOneType))
                   return;
               addOneType.GetCustomAttributes(true).OfType<SortIndexAttribute>().ToList().ForEach(addOne =>
               {
                   addOne.AddServices(services, addOneType);
               });
           });
    }

    private void ConfigViewModelServices(ServiceCollection services)
    {
        Assembly.GetEntryAssembly()!.GetTypes()
            .Where(type => null != type && type.GetCustomAttribute<ViewModelAttribute>() != null)
            .ToList().ForEach(addOneType =>
            {
                if (!CanInjection(addOneType))
                    return;
                addOneType.GetCustomAttributes(true).OfType<ViewModelAttribute>().ToList().ForEach(addOne =>
                {
                    addOne.AddServices(services, addOneType);
                });
            });
    }

    /// <summary>
    /// 是否可以注入
    /// </summary>
    /// <param name="addOneType"></param>
    /// <returns></returns>
    private bool CanInjection(Type addOneType)
    {
        foreach(var att in addOneType.GetCustomAttributes(true))
        {
            if (att is SortIndexAttribute sortIndexAttribute)
                return !sortIndexAttribute.IsDisabled();
        }
        return true;
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var appContainerViewModel = _serviceProvider?.GetRequiredService<AppBootstrap>();
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            desktop.MainWindow = appContainerViewModel!.Init();
        base.OnFrameworkInitializationCompleted();
    }
}
