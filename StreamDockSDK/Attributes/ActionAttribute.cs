namespace StreamDockSDK.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class ActionAttribute : Attribute
{
    public string IconPath { get; set; } = null!;
    public bool UserTitleEnabled { get; set; }
    public bool SupportedInMultiActions { get; set; }
    public string Name { get; set; } = null!;
    public string Tooltip { get; set; } = null!;
    public string PropertyInspectorPath { get; set; } = null!;
}