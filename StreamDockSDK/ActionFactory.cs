using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StreamDockSDK.Extensions;

namespace StreamDockSDK;

internal class ActionFactory
{
    private readonly List<Type> _availableActions;
    private readonly ILogger<ActionFactory> _logger;
    private readonly IServiceProvider _serviceProvider;
    
    public ActionFactory(ILogger<ActionFactory> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _availableActions = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.ExportedTypes)
            .Where(t => t.IsAssignableTo(typeof(IAction)) && !t.IsAbstract)
            .ToList();
        
        logger.LogInformation("Available actions: {@actions}", _availableActions.Select(a => a.FullName));
    }
    
    public IAction? CreateAction(
        string actionName,
        string context,
        Dictionary<string, object> settings,
        Plugin plugin)
    {
        var action = _availableActions.FirstOrDefault(a => a.FullName == actionName);
        if (action is null)
        {
            _logger.LogError("Action {actionName} not found", actionName);
            return null;
        }
        
        var settingsType = action.BaseType!.GenericTypeArguments[0];
        
        _logger.LogInformation("Creating action {actionName}<{actionSettings}>", actionName, settingsType.Name);

        return (IAction)ActivatorUtilities.CreateInstance(
            _serviceProvider,
            action,
            context,
            settings.ToStreamDockJson().FromStreamDockJson(settingsType));
    }
}