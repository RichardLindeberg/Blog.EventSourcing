namespace Blog.EventSourcing.Domain
{
    using System;
    using System.Net;
    using System.Text;

    using EventStore.ClientAPI;
    using EventStore.ClientAPI.Embedded;
    using EventStore.ClientAPI.SystemData;
    using EventStore.Common.Log;
    using EventStore.Common.Options;
    using EventStore.Core;

    public class IPEndPointFactory
    {
        public static IPEndPoint DefaultTcp()
        {
            return CreateIPEndPoint(1113);
        }

        public static IPEndPoint DefaultHttp()
        {
            return CreateIPEndPoint(2113);
        }

        private static IPEndPoint CreateIPEndPoint(int port)
        {
            var address = IPAddress.Parse("127.0.0.1");
            return new IPEndPoint(address, port);
        }
    }


    public class StoreFactory : IStoreFactory
    {
        private static ClusterVNode node = null;

        public IEventStoreConnection GetStore()
        {
            //if (node == null)
            //{
            //    node = CreateStore();
            //}
            //var embeddedConn = EmbeddedEventStoreConnection.Create(node, ConnectionSettings.Create()
            //        .EnableVerboseLogging()
            //        .UseConsoleLogger()
            //        .SetDefaultUserCredentials(new UserCredentials("admin", "changeit"))
            //        .Build());


            var embeddedConn = EventStoreConnection.Create(
                ConnectionSettings.Create()
              //      .EnableVerboseLogging()
                    .UseConsoleLogger()
                    .SetDefaultUserCredentials(new UserCredentials("admin", "changeit"))
                    .Build(),
                IPEndPointFactory.DefaultTcp());
            embeddedConn.ConnectAsync().Wait();
            return embeddedConn;
        }

        private ClusterVNode CreateStore()
        {
            LogManager.Init("eventStore", "./", "./");


            var nodeBuilder = EmbeddedVNodeBuilder.AsSingleNode()
                                        .EnableLoggingOfHttpRequests()
                                      .OnDefaultEndpoints()
                                      .RunProjections(ProjectionType.All)
                                      .StartStandardProjections()
                                      .RunInMemory();

            var node = nodeBuilder.Build();
            node.StartAndWaitUntilReady().Wait();
            Console.WriteLine(node.ExternalHttpService.IsListening);
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