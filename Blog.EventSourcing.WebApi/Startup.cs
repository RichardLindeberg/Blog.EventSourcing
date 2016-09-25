namespace Blog.EventSourcing.WebApi
{
    using System.Web.Http;

    using EventStore.Common.Utils;

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
}