namespace Blog.EventSourcing.Domain
{
    using NEventStore;
    
    public class StoreFactory : IStoreFactory
    {
        private static IStoreEvents store;

        public IStoreEvents CreateStore()
        {
            return Wireup.Init()
                    .UsingInMemoryPersistence()
                    .InitializeStorageEngine()
                    .UsingJsonSerialization()
                    .Build();
        }

        public IStoreEvents GetStore()
        {
            return store ?? (store = CreateStore());
        }
    }
}