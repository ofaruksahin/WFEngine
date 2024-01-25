using Serilog;
using Serilog.Core;
using Serilog.Formatting.Compact;
using WFEngine.Domain.Common.ValueObjects;

namespace WFEngine.Application.Common.Logging
{
    public static class SerilogBootstrapper
    {
        public static Logger GetLoggerConfiguration(LoggingOptions options)
        {
            var logPath = string.Concat(Path.Combine(options.LogDirectory, options.ApplicationName), ".txt");

            return new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Debug(new RenderedCompactJsonFormatter())
                .WriteTo.File(logPath, rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }
    }
}
