namespace Blog.EventSourcing.Domain
{
    using System;
    using System.Text;

    using EventStore.ClientAPI;
    using EventStore.ClientAPI.Embedded;
    using EventStore.Core;



    public class StoreFactory : IStoreFactory
    {
        private static ClusterVNode node = null;

        public IEventStoreConnection GetStore()
        {
            if (node == null)
            {
                node = CreateStore();
            }
            var embeddedConn = EmbeddedEventStoreConnection.Create(node);
            embeddedConn.ConnectAsync().Wait();
            return embeddedConn;
        }

        private ClusterVNode CreateStore()
        {
            var nodeBuilder = EmbeddedVNodeBuilder.AsSingleNode()
                                      .OnDefaultEndpoints()
                                      .RunInMemory();
            var node = nodeBuilder.Build();
            node.StartAndWaitUntilReady().Wait();
            return node;
        }


        //public void Use()
        //{
        //    using (var embeddedConn = EmbeddedEventStoreConnection.Create())
        //    {
        //        embeddedConn.ConnectAsync().Wait();
        //        embeddedConn.AppendToStreamAsync("testStream", ExpectedVersion.Any,
        //                            new EventData(Guid.NewGuid(), "eventType", true,
        //                            Encoding.UTF8.GetBytes("{\"Foo\":\"Bar\"}"), null)).Wait();
        //    }
        //}
    }
}