using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Styling;
using DynamicData;
using FluentAvalonia.Styling;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetX.AppCore.Contract;
using NetX.AppCore.Extentions;
using NetX.AppCore.Models;
using NetX.AppCore.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace NetX.AppCore;

public partial class App : Application
{
    private readonly ServiceCollection _services;
    private ServiceProvider _serviceProvider;
    private IConfiguration _configuration;
    private List<string> _addoneAssemblies;

    public App()
    {
        _services = new ServiceCollection();
    }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
        // Default logic doesn't auto detect windows theme anymore in designer
        // to stop light mode, force here
        if (Design.IsDesignMode)
            RequestedThemeVariant = ThemeVariant.Dark;
        ConfigureJsonConfig();
        ConfigureServices(_services);
        ConfigAddOneServices(_services);
        _serviceProvider = _services.BuildServiceProvider();
    }

    private void ConfigureJsonConfig()
    {
        var builder = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile($"{Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AppConst.APP_CONFIG_FILE)} ")
              .AddJsonFile($"{Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AppConst.APP_CONFIG_USER_FILE)} ")
              .AddJsonFile($"{Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AppConst.APP_CONFIG_ADDONE_FILE)} ");
        _configuration = builder.Build();
    }

    private void ConfigureServices(ServiceCollection services)
    {
        // add gloable config
        services.Configure<AppUserConfig>(_configuration)
            .AddOptions<AppUserConfig>();
        services.Configure<AppConfig>(_configuration)
            .AddOptions<AppConfig>();
        services.Configure<AppAddoneConfig>(_configuration)
            .AddOptions<AppAddoneConfig>();
        services.AddSingleton<IConfiguration>(_configuration);
        _addoneAssemblies = _configuration.Get<AppAddoneConfig>()?.AddoneAssembly?.ToList();
        if(null == _addoneAssemblies)
            _addoneAssemblies = new List<string>();
        _addoneAssemblies.Add(Assembly.GetExecutingAssembly().GetName().Name);

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
        //services.AddTransient<IStartupWindowViewModel,WorkbenchViewModel>();
        //services.AddSingleton<SettingPageViewModel>();
    }

    private void ConfigAddOneServices(ServiceCollection services)
    {
        ConfigInitialize(services);
        ConfigViewModelServices(services);
        ConfigEventBusServices(services);
    }

    private void ConfigInitialize(ServiceCollection services)
    {
        var types = GetAllType<IAddoneInitializer>();
        foreach (var type in types)
        {
            var instance = Activator.CreateInstance(type);
            if (instance is IAddoneInitializer initializer)
                initializer.ConfigureServices(services);
        }
    }

    private void ConfigEventBusServices(ServiceCollection services)
    {
        var types = GetAllTypes<EventBusHanderAttribute>();
        foreach (var type in types)
        {
            foreach (var interfaceType in type.GetInterfaces())
            {
                if (interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == typeof(INotificationHandler<>))
                    services.AddSingleton(interfaceType, type);
            }
        }
    }

    //private void ConfigStartStepServices(ServiceCollection services)
    //{
    //    GetAllTypes<SortIndexAttribute>()
    //        .Where(type => null != type.GetCustomAttribute<SortIndexAttribute>())
    //        .OrderBy(type => type.GetCustomAttribute<SortIndexAttribute>()!.Order)
    //        .ToList().ForEach(addOneType =>
    //        {
    //            if (!CanInjection(addOneType))
    //                return;
    //            addOneType.GetCustomAttributes(true).OfType<SortIndexAttribute>().ToList().ForEach(addOne =>
    //            {
    //                addOne.AddServices(services, addOneType);
    //            });
    //        });
    //}

    private void ConfigViewModelServices(ServiceCollection services)
    {
        GetAllTypes<ViewModelAttribute>()
           .Where(type => null != type && type.GetCustomAttribute<ViewModelAttribute>() != null)
            .ToList().ForEach(addOneType =>
            {
                addOneType.GetCustomAttributes(true).OfType<ViewModelAttribute>().ToList().ForEach(addOne =>
                {
                    addOne.AddServices(services, addOneType);
                });
            });
    }

    private IEnumerable<Type> GetAllTypes<TAttribute>()
        where TAttribute : Attribute
    {
        var allType = new List<Type>();
        var coreTypes = Assembly.GetEntryAssembly()!.GetTypesWithAttribute<TAttribute>();
        allType.AddRange(coreTypes);
        foreach (var assembly in _addoneAssemblies)
        {
            var addoneTypes = Assembly.Load(assembly).GetTypesWithAttribute<TAttribute>();
            if (addoneTypes.Any())
                allType.AddRange(addoneTypes);
        }
        return allType.Distinct();
    }

    private IEnumerable<Type> GetAllType<TInterface>()
    {
        var allType = new List<Type>();
        foreach (var assembly in _addoneAssemblies)
        {
            var addoneTypes = Assembly.Load(assembly).GetTypeWithInterface<TInterface>();
            if (addoneTypes.Any())
                allType.AddRange(addoneTypes);
        }
        return allType.Distinct();
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var appContainerViewModel = _serviceProvider?.GetRequiredService<AppBootstrap>();
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            desktop.MainWindow = appContainerViewModel!.Init();
        base.OnFrameworkInitializationCompleted();
    }
}
