using Avalonia;
using Avalonia.ReactiveUI;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace NetX.AppContainer.ClassicDesktop
{
    public static class Bootstrap
    {
        public static int Startup(string[] args) => BuildAvaloniaApp()
        .StartWithClassicDesktopLifetime(args);

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>(()=> CreateApp())
                .UsePlatformDetect()
                .WithInterFont()
                .LogToTrace()
                .UseReactiveUI();

        /// <summary>
        /// 构建App
        /// </summary>
        /// <returns></returns>
        private static App CreateApp()
        {
            var services = new ServiceCollection();
            var addoneType = Assembly.GetEntryAssembly()!.EntryPoint!.DeclaringType;
            if(null == addoneType)
                throw new EntryPointNotFoundException("未找到入口类型");
            return new App(addoneType!, services);
        }
    }
}
