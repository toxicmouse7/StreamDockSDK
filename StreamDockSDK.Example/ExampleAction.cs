using System.Diagnostics;
using StreamDockSDK.Attributes;

namespace StreamDockSDK.Example;

[Action(
    Name = "Example",
    Tooltip = "Example Action",
    IconPath = "Assets/Images/App-logo.png",
    SupportedInMultiActions = false,
    UserTitleEnabled = false,
    PropertyInspectorPath = "Assets/PropertyInspector")]
[State(Image = "static/img/App-logo.png")]
public class ExampleAction : AbstractAction
{
    private string _scriptPath = null!;

    public ExampleAction(string context, Dictionary<string, object> settings, Action<object> sender)
        : base(context, settings, sender)
    {
        SetTitle("xXx");

        if (settings.TryGetValue("scriptPath", out var setting))
        {
            _scriptPath = (string)setting;
        }
    }

    public override void OnKeyDown(Payload payload)
    {
        var process = Process.Start(new ProcessStartInfo(
            "python.exe",
            [_scriptPath]
        ));
        
        process?.WaitForExit();
    }

    public override void OnSendToPlugin(Message message)
    {
        var settings = message.Payload.Settings;
        _scriptPath = (string)settings["scriptPath"];

        SetSettings(message.Payload.Settings);
    }
}