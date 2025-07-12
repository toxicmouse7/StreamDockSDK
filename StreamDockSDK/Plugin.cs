using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StreamDockSDK.Bus;
using StreamDockSDK.BusEvents;

namespace StreamDockSDK;

public sealed class Plugin : IHostedService
{
    private readonly int _port;
    private readonly string _uuid;
    private readonly string _registerEvent;
    
    private readonly ILogger<Plugin> _logger;
    private readonly ActionFactory _actionFactory;
    private readonly IBus _bus;

    private readonly Dictionary<string, IAction> _actions = [];

    private Dictionary<string, object> _globalSettings = [];

    public Plugin(
        int port,
        string uuid,
        string registerEvent,
        IServiceProvider serviceProvider,
        IBus bus)
    {
        _uuid = uuid;
        _registerEvent = registerEvent;
        _port = port;
        _bus = bus;

        _logger = serviceProvider.GetRequiredService<ILogger<Plugin>>();
        _actionFactory = serviceProvider.GetRequiredService<ActionFactory>();
        
        _bus.Subscribe<MessageReceived>(async @event => await OnMessageReceivedAsync(@event.Message));
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting StreamDockSDK");
        _bus.Publish(new PluginActivated(_port, _uuid, _registerEvent));
        
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private async Task OnMessageReceivedAsync(Message message)
    {
        await Task.CompletedTask;
        _logger.LogInformation("Event: {@event}", message);

        switch (message.Event)
        {
            case Events.PluginEvents.DidReceiveGlobalSettings:
            {
                _globalSettings = message.Payload.Settings;
                foreach (var action in _actions.Values)
                {
                    action.OnDidReceiveGlobalSettings(_globalSettings);
                }

                return;
            }
            case Events.PluginEvents.WillAppear:
            {
                if (_actions.ContainsKey(message.Context!)) return;

                var newAction = _actionFactory.CreateAction(
                    message.Action!,
                    message.Context!,
                    message.Payload.Settings,
                    this);

                if (newAction is null)
                {
                    return;
                }

                _actions[message.Context!] = newAction;

                return;
            }
            case Events.PluginEvents.WillDisappear:
            {
                if (!_actions.Remove(message.Context!, out var action))
                {
                    return;
                }

                action.OnWillDisappear();
                return;
            }
            case Events.PluginEvents.DidReceiveSettings:
            {
                if (!_actions.TryGetValue(message.Context!, out var action))
                {
                    return;
                }

                action.OnDidReceiveSettings(message.Payload.Settings);

                return;
            }
            case Events.PluginEvents.TitleParametersDidChange:
            {
                if (!_actions.TryGetValue(message.Context!, out var action))
                {
                    return;
                }

                action.OnTitleParametersDidChange(message.Payload);

                return;
            }
        }

        {
            if (!_actions.TryGetValue(message.Context ?? string.Empty, out var action))
            {
                return;
            }

            var contextHandler = message.Event switch
            {
                Events.ContextEvents.KeyUp => action.OnKeyUp,
                Events.ContextEvents.KeyDown => action.OnKeyDown,
                Events.ContextEvents.DialUp => action.OnDialUp,
                Events.ContextEvents.DialDown => action.OnDialDown,
                Events.ContextEvents.DialRotate => action.OnDialRotate,
                _ => (Action<Payload>?)null
            };

            if (contextHandler is not null)
            {
                contextHandler(message.Payload);
                return;
            }

            var globalHandler = message.Event switch
            {
                Events.GlobalEvents.DeviceDidConnect => action.OnDeviceDidConnect,
                Events.GlobalEvents.DeviceDidDisconnect => action.OnDeviceDidDisconnect,
                Events.GlobalEvents.ApplicationDidLaunch => action.OnApplicationDidLaunch,
                Events.GlobalEvents.ApplicationDidTerminate => action.OnApplicationDidTerminate,
                Events.GlobalEvents.SystemDidWakeUp => action.OnSystemDidWakeUp,
                _ => (Action<Message>?)null
            };

            if (globalHandler is not null)
            {
                globalHandler(message);
                return;
            }

            var interactionHandler = message.Event switch
            {
                Events.InteractionEvents.PropertyInspectorDidAppear => action.OnPropertyInspectorDidAppear,
                Events.InteractionEvents.PropertyInspectorDidDisappear => action.OnPropertyInspectorDidDisappear,
                Events.InteractionEvents.SendToPlugin => action.OnSendToPlugin,
                _ => (Action<Message>?)null
            };

            interactionHandler?.Invoke(message);
        }
    }
}