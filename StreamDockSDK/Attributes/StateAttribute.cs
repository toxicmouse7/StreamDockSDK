namespace StreamDockSDK.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class StateAttribute : Attribute
{
    public required string Image { get; set; } = null!;
}