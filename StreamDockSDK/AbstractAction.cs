namespace StreamDockSDK;

public abstract class AbstractAction
{
    private readonly string _context;
    private readonly Action<object> _sender;

    public AbstractAction(string context, Dictionary<string, object> settings, Action<object> sender)
    {
        _context = context;
        _sender = sender;
    }

    public void SetTitle(string title)
    {
        _sender(new
        {
            Event = "setTitle",
            Context = _context,
            Payload = new
            {
                Title = title,
                Target = 0
            }
        });
    }

    public void SetState(int stateNumber)
    {
        _sender(new
        {
            Event = "setState",
            Context = _context,
            Payload = new
            {
                State = stateNumber
            }
        });
    }

    public void SetSettings(Dictionary<string, object> settings)
    {
        _sender(new
        {
            Event = "setSettings",
            Context = _context,
            Payload = settings
        });
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