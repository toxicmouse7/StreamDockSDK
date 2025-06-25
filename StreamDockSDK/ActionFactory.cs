using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

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
            .Where(t => t.IsAssignableTo(typeof(AbstractAction)) && !t.IsAbstract)
            .ToList();
        
        logger.LogInformation("Available actions: {@actions}", _availableActions.Select(a => a.FullName));
    }
    
    public AbstractAction? CreateAction(
        string actionName,
        string context,
        Dictionary<string, object> settings,
        Plugin plugin)
    {
        var action = _availableActions.FirstOrDefault(a => a.FullName == actionName);
        if (action is null)
        {
            return null;
        }
        
        _logger.LogInformation("Creating action {actionName}", actionName);

        return ActivatorUtilities.CreateInstance(
            _serviceProvider,
            action,
            context,
            settings,
            (Action<object>)plugin.Send) as AbstractAction;
    }
}