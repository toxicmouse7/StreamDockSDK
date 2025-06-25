using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace StreamDockSDK;

public static class DependencyInjection
{
    public static IServiceCollection AddStreamDockSDK(
        this IServiceCollection services,
        int port,
        string pluginUUID,
        string registerEvent)
    {
        services.AddHostedService(provider => new Plugin(
            port,
            pluginUUID,
            registerEvent,
            provider));
        
        services.AddSingleton<ActionFactory>();

        return services;
    }
}