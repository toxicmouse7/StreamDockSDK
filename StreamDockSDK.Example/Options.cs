using CommandLine;

namespace StreamDockSDK.Example;

public class Options
{
    [Option("port", Required = true, HelpText = "WebSocket port")]
    public int Port { get; set; }

    [Option("pluginUUID", Required = true, HelpText = "Plugin UUID")]
    public string PluginUUID { get; set; } = null!;

    [Option("registerEvent", Required = true, HelpText = "Event type for plugin registration")]
    public string RegisterEvent { get; set; } = null!;

    [Option("info", Required = true, HelpText = "JSON string containing Stream Dock and device information")]
    public string Info { get; set; } = null!;
}