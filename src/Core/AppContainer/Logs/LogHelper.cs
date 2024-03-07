using Avalonia.Logging;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetX.AppContainer.Logs
{
    public class LogHelper
    {
        public static void RegisterLog()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(AppConst.APP_CONFIG_LOG_FILE, optional: true, reloadOnChange: true)
                .Build();
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(config)
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
