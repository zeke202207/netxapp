using Avalonia;
using Avalonia.Logging;
using Avalonia.ReactiveUI;
using NetX.AppContainer;
using NetX.AppContainer.Extentions;
using NetX.AppContainer.Logs;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDemo;

public class Program
{
    //不能包装，包装后设计器无法使用,BuildAvaloniaApp所在的类必须引用设计器类库，才能在类库中使用设计器 
    //[STAThread]
    //public static void Main(string[] args) => Bootstrap.Startup(args);

    // Initialization code.Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        try
        {
            LogHelper.RegisterLog();
            BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);
        }
        catch (Exception ex)
        {
            Log.Fatal(ex,ex.ToString());
        }
        finally
        {
            LogHelper.CloseAndFlush();
        }
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<MyDemoApp>()
        .UsePlatformDetect()
        .UseAlibabaFont()
        .WithInterFont()
        .UseLog()
        .UseReactiveUI();
}
