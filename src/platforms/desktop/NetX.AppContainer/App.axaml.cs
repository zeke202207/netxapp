using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Templates;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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

        var viewlocator = Current?.DataTemplates.First(x => x is ViewLocator);
        if (viewlocator is not null)
            _services.AddSingleton(viewlocator);

        _services.AddSingleton<IControlCreator, ActivatorControlCreator>();
        //viewmodel
        _services.AddSingleton<AppContainerViewModel>();
        _services.AddSingleton<MainViewModel>();
        _services.AddSingleton<IStartupWindowViewModel,MainViewModel>();
    }

    private void ConfigAddOneServices()
    {
        ConfigStartStepServices();
        ConfigViewModelServices();
    }

    private void ConfigStartStepServices()
    {
        Assembly.GetEntryAssembly()!.GetTypes()
           .Where(type => null != type.GetCustomAttribute<SortIndexAttribute>())
           .OrderBy(type => type.GetCustomAttribute<SortIndexAttribute>()!.Order)
           .ToList().ForEach(addOneType =>
           {
               addOneType.GetCustomAttributes(true).OfType<SortIndexAttribute>().ToList().ForEach(addOne =>
               {
                   if (!addOne.IsDisabled())
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
        var dataTemplate = _serviceProvider?.GetRequiredService<IDataTemplate>();
        var appContainerViewModel = _serviceProvider?.GetRequiredService<AppContainerViewModel>();
        var splashScreenWindows = dataTemplate.Build(appContainerViewModel) as SukiWindow;
        if (null == splashScreenWindows)
            return;
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = appContainerViewModel!.Init();
        }

        base.OnFrameworkInitializationCompleted();
    }
}
