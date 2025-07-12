namespace StreamDockSDK.Bus;

internal class Bus : IBus
{
    private readonly List<Delegate> _handlers = [];
    
    public IBus Subscribe<TEvent>(Func<TEvent, Task> handler)
    {
        _handlers.Add(handler);

        return this;
    }

    public void Publish<TEvent>(TEvent @event)
    {
        _ = Task.Run(async () => await PublishAsync(@event));
    }

    private async Task PublishAsync<TEvent>(TEvent @event)
    {
        var eventHandlers = _handlers.OfType<Func<TEvent, Task>>();

        var tasks = eventHandlers.Select(eventHandler => eventHandler.Invoke(@event)).ToList();

        await Task.WhenAll(tasks);
    }
}