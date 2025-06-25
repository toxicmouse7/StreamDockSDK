namespace StreamDockSDK;

public class Payload
{
    public Dictionary<string, object> Settings { get; init; } = [];
}

public class Message
{
    public string Event { get; init; } = null!;
    public Payload Payload { get; init; } = null!;
    public string? Context { get; init; }
    public string? Action { get; init; }
    public string? Uuid { get; init; }
}