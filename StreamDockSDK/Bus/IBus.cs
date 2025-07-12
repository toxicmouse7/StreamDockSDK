namespace StreamDockSDK.Bus;


public interface IBus
{
    IBus Subscribe<TEvent>(Func<TEvent, Task> handler);
    void Publish<TEvent>(TEvent @event);
}