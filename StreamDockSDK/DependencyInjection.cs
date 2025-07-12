using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StreamDockSDK.Bus;
using WebSocketSharp;

namespace StreamDockSDK;

public static class DependencyInjection
{
    public static IServiceCollection AddStreamDockSDK(
        this IServiceCollection services,
        int port,
        string pluginUUID,
        string registerEvent)
    {
        services.AddHostedService(provider => 
            ActivatorUtilities.CreateInstance<Plugin>(provider, port, pluginUUID, registerEvent));
        
        services.AddSingleton<ActionFactory>();
        services.AddSingleton<IBus, Bus.Bus>();
        services.AddActivatedSingleton<WebSocketClient>();

        return services;
    }
}