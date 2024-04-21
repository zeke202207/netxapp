using Avalonia.Logging;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Settings.Configuration;
using System.IO;

namespace NetX.AppCore.Logs
{
    public class LogHelper
    {
        public static void RegisterLog()
        {
            var options = new ConfigurationReaderOptions(typeof(ConsoleLoggerConfigurationExtensions).Assembly, typeof(FileLoggerConfigurationExtensions).Assembly);
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(AppConst.APP_CONFIG_LOG_FILE, optional: true, reloadOnChange: true)
                .Build();
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(config, options)
            .CreateLogger();
            //设置Avalonia使用SerilogLogSink
            Logger.Sink = new SerilogLogSink();
        }

        public static void CloseAndFlush()
        {
            Log.CloseAndFlush();
        }
    }
}
