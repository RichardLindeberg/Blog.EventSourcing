namespace Blog.EventSourcing.WebApi
{
    using System;
    using System.Web.Http;

    using Blog.EventSourcing.Domain;
    using Blog.EventSourcing.SimpleReadModell;
    using EventStore.Common.Utils;
    using EventStore.ClientAPI;

    using Owin;

    public class Startup
    {
        // This code configures Web API. The Startup class is specified as a type
        // parameter in the WebApp.Start method.
        public void Configuration(IAppBuilder appBuilder)
        {
            // Configure Web API for self-host. 
            HttpConfiguration config = new HttpConfiguration();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
                );

            var iDOntCare = ShittyStaticReadModell.People.Value;

            appBuilder.UseWebApi(config);
        }
    }

    public class ShittyStaticReadModell
    {
        public static Lazy<PeopleReadModell> People = new Lazy<PeopleReadModell>(() => Subscribe());

        private static PeopleReadModell Subscribe()
        {
            var store = new StoreFactory().GetStore();
          
            var peopleModell = new PeopleReadModell();
                
            var s = store.SubscribeToStreamFrom(
                "people",
                null,
               CatchUpSubscriptionSettings.Default,
               (subscription, @event) => peopleModell.Handle(@event.Event.ToIPersonEvent()),
               subscription => Console.WriteLine("Started catchup"),
               (subscription, reason, arg3) => Console.WriteLine("Dropped subscription"));
            return peopleModell;
        }
    }
}