using System.Diagnostics;
using StreamDockSDK.Attributes;
using StreamDockSDK.Bus;
using StreamDockSDK.Extensions;

namespace StreamDockSDK.Example.Actions.ExampleAction;

[Action(
    Name = "Example",
    Tooltip = "Example Action",
    IconPath = "Assets/Images/App-logo.png",
    SupportedInMultiActions = false,
    UserTitleEnabled = false,
    PropertyInspectorPath = "Assets/PropertyInspector/Example/index.html")]
[State(Image = "static/img/App-logo.png")]
public class ExampleAction : AbstractAction<ExampleActionSettings>
{
    private ExampleActionSettings _settings;

    public ExampleAction(
        string context,
        ExampleActionSettings settings,
        IBus bus)
        : base(context, settings, bus)
    {
        _settings = settings;
        SetTitle("xXx");
    }

    public override void OnKeyDown(Payload payload)
    {
        if (_settings.ScriptPath is null)
        {
            return;
        }
        
        var process = Process.Start(new ProcessStartInfo(
            "python.exe",
            [_settings.ScriptPath]
        ));
        
        process?.WaitForExit();
    }

    public override void OnSendToPlugin(Message message)
    {
        var settings = message.Payload.Settings;
        _settings = settings.ToStreamDockJson().FromStreamDockJson<ExampleActionSettings>();

        SetSettings(_settings);
    }
}