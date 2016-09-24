namespace Blog.EventSourcing.Domain
{
    using EventStore.ClientAPI;

    public interface IStoreFactory
    {
        IEventStoreConnection GetStore();
    }
}