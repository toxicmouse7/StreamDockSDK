using Microsoft.Extensions.Logging;
using StreamDockSDK.Bus;
using StreamDockSDK.BusEvents;
using StreamDockSDK.Extensions;
using WebSocketSharp;

namespace StreamDockSDK;

public class WebSocketClient
{
    private WebSocket _client = null!;
    private readonly IBus _bus;
    private readonly ILogger<WebSocketClient> _logger;

    public WebSocketClient(IBus bus, ILogger<WebSocketClient> logger)
    {
        _bus = bus;
        _logger = logger;
        _bus
            .Subscribe<PluginActivated>(ConnectToServer)
            .Subscribe<SendToServer>(@event => SendToServer(@event.Message));
    }

    private Task ConnectToServer(PluginActivated pluginActivated)
    {
        _client = new WebSocket($"ws://127.0.0.1:{pluginActivated.Port}");
        
        _client.OnOpen += (_, _) =>
        {
            var message = new Message
            {
                Event = pluginActivated.RegisterEvent,
                Uuid = pluginActivated.Uuid
            };

            _client.Send(message.ToStreamDockJson());
        };

        _client.OnMessage += (_, e) =>
        {
            var message = e.Data.FromStreamDockJson<Message>();
            _bus.Publish(new MessageReceived(message));
        };

        _client.OnError += (_, args) => _logger.LogError(args.Exception, "WebSocket error");
        
        _client.Connect();

        return Task.CompletedTask;
    }

    private Task SendToServer(object message)
    {
        _client.Send(message.ToStreamDockJson());

        return Task.CompletedTask;
    }
}