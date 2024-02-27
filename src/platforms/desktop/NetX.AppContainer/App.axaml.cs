using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Templates;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetX.AppContainer.Common;
using NetX.AppContainer.Contract;
using NetX.AppContainer.Extentions;
using NetX.AppContainer.Models;
using NetX.AppContainer.ViewModels;
using NetX.AppContainer.Views;
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
    private readonly Type _addOneType;
    private readonly ServiceCollection _services;
    private ServiceProvider _serviceProvider;
    private IConfiguration _configuration;

    public App(Type addOneType, ServiceCollection services)
    {
        _addOneType = addOneType;
        _services = services;
    }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
        ConfigureJsonConfig();
        ConfigureServices();
        ConfigAddOneServices();
        _serviceProvider = _services.BuildServiceProvider();
    }

    private void ConfigureJsonConfig()
    {
        var builder = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile($"{Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsetting.json")} ");
        _configuration = builder.Build();
    }

    private void ConfigureServices()
    {
        // add gloable config
        _services.Configure<AppConfig>(_configuration)
            .AddOptions<AppConfig>();
        // viewlocator
        _services.AddSingleton<IControlCreator, ControlCreator>();
        //viewmodel
        _services.AddSingleton<AppContainerViewModel>();
        _services.AddSingleton<MainViewModel>();
        _services.AddSingleton<IStartupViewModel,MainViewModel>();
    }

    private void ConfigAddOneServices()
    {
        ConfigStartStepServices();
        ConfigViewModelServices();
    }

    private void ConfigStartStepServices()
    {
        Assembly.GetEntryAssembly()!.GetTypes()
           .Where(type => null != type.GetCustomAttribute<StartStepAttribute>())
           .OrderBy(type => type.GetCustomAttribute<StartStepAttribute>()!.Order)
           .ToList().ForEach(addOneType =>
           {
               addOneType.GetCustomAttributes(true).OfType<StartStepAttribute>().ToList().ForEach(addOne =>
               {
                   addOne.AddServices(_services, addOneType);
               });
           });
    }

    private void ConfigViewModelServices()
    {
        Assembly.GetEntryAssembly()!.GetTypes()
            .Where(type => null != type && type.GetCustomAttribute<ViewModelAttribute>() != null)
            .ToList().ForEach(addOneType =>
            {
                addOneType.GetCustomAttributes(true).OfType<ViewModelAttribute>().ToList().ForEach(addOne =>
                {
                    addOne.AddServices(_services, addOneType);
                });
            });
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var controlCreator = _serviceProvider?.GetRequiredService<IControlCreator>();
        var splashScreenViewModel = _serviceProvider?.GetRequiredService<AppContainerViewModel>();
        var splashScreenWindows = splashScreenViewModel.CreateView() as SukiWindow;
        if (null == splashScreenWindows)
            return;
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = splashScreenViewModel!.Init();
        }

        base.OnFrameworkInitializationCompleted();
    }
}
