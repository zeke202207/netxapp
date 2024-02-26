using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Templates;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetX.AppContainer.Common;
using NetX.AppContainer.Extentions;
using NetX.AppContainer.Models;
using NetX.AppContainer.ViewModels;
using NetX.AppContainer.Views;
using SukiUI.Controls;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetX.AppContainer;

public partial class App : Application
{
    private IServiceProvider? _serviceProvider;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
        _serviceProvider = this.CreateServiceProvider(s =>
        {
            InitAppContainer(s);
            InitAddOne(s);
        });
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var viewLocator = _serviceProvider?.GetRequiredService<IDataTemplate>();
        var splashScreenViewModel = _serviceProvider?.GetRequiredService<SplashScreenViewModel>();
        var splashScreenWindows = viewLocator?.Build(splashScreenViewModel) as SukiWindow;
        if (null == splashScreenWindows)
            return;
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = splashScreenWindows;
            splashScreenViewModel!.Init(splashScreenWindows);
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void InitAppContainer(ServiceCollection services)
    {
        // add gloable config
        var builder = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile($"{Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsetting.json")} ");
        services.Configure<AppConfig>(builder.Build())
            .AddOptions<AppConfig>();

        //viewmodel
        services.AddSingleton<SplashScreenViewModel>();
        services.AddSingleton<LoginViewModel>();
        services.AddSingleton<MainViewModel>();

        //model
    }

    private void InitAddOne(ServiceCollection services)
    {

    }
}
