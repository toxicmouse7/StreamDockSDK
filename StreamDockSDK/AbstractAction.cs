using StreamDockSDK.Bus;
using StreamDockSDK.BusEvents;

namespace StreamDockSDK;

public abstract class AbstractAction<TActionSettings> : IAction
{
    private readonly string _context;
    private readonly IBus _bus;

    public AbstractAction(string context, TActionSettings settings, IBus bus)
    {
        _context = context;
        _bus = bus;
    }

    protected void SetTitle(string title)
    {
        _bus.Publish(new SendToServer(new
        {
            Event = "setTitle",
            Context = _context,
            Payload = new
            {
                Title = title,
                Target = 0
            }
        }));
    }

    protected void SetState(int stateNumber)
    {
        _bus.Publish(new SendToServer(new
        {
            Event = "setState",
            Context = _context,
            Payload = new
            {
                State = stateNumber
            }
        }));
    }

    protected void SetSettings(TActionSettings settings)
    {
        _bus.Publish(new SendToServer(new
        {
            Event = "setSettings",
            Context = _context,
            Payload = settings
        }));
    }

    public virtual void OnDidReceiveGlobalSettings(Dictionary<string, object> settings)
    {
    }

    public virtual void OnWillDisappear()
    {
    }

    public virtual void OnDidReceiveSettings(Dictionary<string, object> payloadSettings)
    {
    }

    public virtual void OnTitleParametersDidChange(Payload messagePayload)
    {
    }

    public virtual void OnKeyUp(Payload payload)
    {
    }

    public virtual void OnKeyDown(Payload payload)
    {
    }

    public virtual void OnDialUp(Payload payload)
    {
    }

    public virtual void OnDialDown(Payload payload)
    {
    }

    public virtual void OnDialRotate(Payload payload)
    {
    }

    public virtual void OnDeviceDidConnect(Message message)
    {
    }

    public virtual void OnDeviceDidDisconnect(Message message)
    {
    }

    public virtual void OnApplicationDidLaunch(Message message)
    {
    }

    public virtual void OnApplicationDidTerminate(Message message)
    {
    }

    public virtual void OnSystemDidWakeUp(Message message)
    {
    }

    public virtual void OnPropertyInspectorDidAppear(Message message)
    {
    }

    public virtual void OnPropertyInspectorDidDisappear(Message message)
    {
    }

    public virtual void OnSendToPlugin(Message message)
    {
    }
}