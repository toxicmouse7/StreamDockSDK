using CommandLine;
using Microsoft.Extensions.Hosting;
using Serilog;
using StreamDockSDK;
using StreamDockSDK.Example;

var pluginPath = Path.GetDirectoryName(Environment.ProcessPath);

var logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File(
        Path.Join(pluginPath, "logs/plugin.log"),
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 1)
    .CreateLogger();

logger.Information("Run plugin with args: {@args}", args);


var transformedArgs = args.Select(arg =>
{
    if (arg.StartsWith('-'))
    {
        return '-' + arg;
    }

    return arg;
});

var result = Parser.Default.ParseArguments<Options>(transformedArgs);

var builder = Host.CreateEmptyApplicationBuilder(null);

builder.Logging.AddSerilog(logger);

builder.Services.AddStreamDockSDK(
    result.Value.Port,
    result.Value.PluginUUID,
    result.Value.RegisterEvent);

var app = builder.Build();

app.Run();