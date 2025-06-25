using System.Text.Json;
using System.Text.Json.Serialization;
using StreamDockSDK.Example;
using StreamDockSDK.ManifestBuilder;

var manifestBuilder = new ManifestBuilder<PluginAssembly>();

var manifest = manifestBuilder.Build(
    "Aleksej",
    "Example Category",
    "Example",
    "1.0.0",
    "Example of plugin",
    "Assets/Images/App-logo.png",
    [
        new SupportedOperatingSystem
        {
            Platform = SupportedOperatingSystem.OperatingSystem.Windows,
            MinimumVersion = "7"
        }
    ]);

Console.WriteLine(JsonSerializer.Serialize(manifest, new JsonSerializerOptions
{
    WriteIndented = true,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
}));