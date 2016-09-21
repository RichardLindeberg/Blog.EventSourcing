namespace Blog.EventSourcing.Domain
{
    using NEventStore;

    public interface IStoreFactory
    {
        IStoreEvents CreateStore();

        IStoreEvents GetStore();
    }
}