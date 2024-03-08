using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Templates;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Threading;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetX.AppCore.Contract;
using NetX.AppCore.Extentions;
using NetX.AppCore.Models;
using NetX.AppCore.ViewModels;
using NetX.AppCore.Views;
using SukiUI;
using SukiUI.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace NetX.AppCore;

public partial class App : Application
{
    private readonly ServiceCollection _services;
    private ServiceProvider _serviceProvider;
    private IConfiguration _configuration;
    private string[] _addoneAssemblies;

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
        _addoneAssemblies = _configuration.Get<AppAddoneConfig>()?.AddoneAssembly;

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
        GetAllTypes<SortIndexAttribute>()
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
        GetAllTypes<ViewModelAttribute>()
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

    private IEnumerable<Type> GetAllTypes<TAttribute>()
        where TAttribute : Attribute
    {
        var allType = new List<Type>();
        var coreTypes = Assembly.GetEntryAssembly()!.GetTypesWithAttribute<SortIndexAttribute>();
        allType.AddRange(coreTypes);
        foreach (var assembly in _addoneAssemblies)
        {
            var addoneTypes = Assembly.Load(assembly).GetTypesWithAttribute<SortIndexAttribute>();
            if (addoneTypes.Any())
                allType.AddRange(addoneTypes);
        }
        return allType;
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
        var config = _configuration.Get<AppUserConfig>();
        InitTheme(config);
        var appContainerViewModel = _serviceProvider?.GetRequiredService<AppBootstrap>();
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            desktop.MainWindow = appContainerViewModel!.Init();
        base.OnFrameworkInitializationCompleted();
    }

    private void InitTheme(AppUserConfig config)
    {
        if (null == config)
            return;
        var theme = SukiTheme.GetInstance();
        var colorTheme = new SukiUI.Models.SukiColorTheme(
                                    config.Themes.ThemeColor.DisplayName,
                                    Color.Parse(config.Themes.ThemeColor.Primary),
                                    Color.Parse(config.Themes.ThemeColor.Accent)
                                    );
        if(theme.ColorThemes.FirstOrDefault(p=>p.DisplayName.ToLower() == colorTheme.DisplayName.ToLower()) == null)
            theme.AddColorTheme(colorTheme);
        theme.ChangeColorTheme(colorTheme);
        Application.Current.RequestedThemeVariant = config.Themes.Theme;

    }
}
