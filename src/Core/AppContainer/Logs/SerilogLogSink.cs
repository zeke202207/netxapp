using Avalonia.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetX.AppContainer.Logs
{
    internal class SerilogLogSink : ILogSink
    {
        public SerilogLogSink()
        {
        }

        public bool IsEnabled(LogEventLevel level, string area) => Serilog.Log.IsEnabled(ToSerilogLevel(level));

        public void Log(LogEventLevel level, string area, object? source, string messageTemplate) => Log(level,area,source,messageTemplate,null);

        public void Log(LogEventLevel level, string area, object? source, string messageTemplate, params object?[] propertyValues)
        {
            Serilog.Log.Logger.Write(ToSerilogLevel(level), messageTemplate, propertyValues);
        }

        private Serilog.Events.LogEventLevel ToSerilogLevel(LogEventLevel level)
        {
            return level switch
            {
                LogEventLevel.Verbose => Serilog.Events.LogEventLevel.Verbose,
                LogEventLevel.Debug => Serilog.Events.LogEventLevel.Debug,
                LogEventLevel.Information => Serilog.Events.LogEventLevel.Information,
                LogEventLevel.Warning => Serilog.Events.LogEventLevel.Warning,
                LogEventLevel.Error => Serilog.Events.LogEventLevel.Error,
                LogEventLevel.Fatal => Serilog.Events.LogEventLevel.Fatal,
                _ => Serilog.Events.LogEventLevel.Information,
            };
        }
    }
}
