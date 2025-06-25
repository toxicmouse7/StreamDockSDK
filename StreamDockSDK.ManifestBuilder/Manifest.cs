namespace StreamDockSDK.ManifestBuilder;

public class SupportedOperatingSystem
{
    public enum OperatingSystem
    {
        Mac,
        Windows
    }

    public required OperatingSystem Platform { get; set; }
    public required string MinimumVersion { get; set; }
}

public class State
{
    public required string Image { get; set; } = null!;
}

public class Action
{
    public required string UUID { get; set; } = null!;
    public required string Icon { get; set; } = null!;
    public required IEnumerable<State> States { get; set; } = null!;
    public bool UserTitleEnabled { get; set; }
    public bool SupportedInMultiActions { get; set; }
    public IEnumerable<string> Controllers { get; set; } = null!;
    public required string Name { get; set; } = null!;
    public string Tooltip { get; set; } = null!;
    public string PropertyInspectorPath { get; set; } = null!;
}

public class Manifest
{
    public int SDKVersion { get; } = 1;
    public required string Version { get; set; } = null!;
    public required string Name { get; set; } = null!;
    public required string Description { get; set; } = null!;
    public string Category { get; set; } = null!;
    public required string Icon { get; set; } = null!;
    public required string CodePath { get; set; } = null!;
    public required string Author { get; set; } = null!;
    public required IEnumerable<Action> Actions { get; set; } = null!;

    public required IEnumerable<SupportedOperatingSystem> OS { get; set; } = null!;
}