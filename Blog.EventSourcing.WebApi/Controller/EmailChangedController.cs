namespace Blog.EventSourcing.WebApi.Controller
{
    using System.Collections.Generic;
    using System.Web.Http;

    using Blog.EventSourcing.SimpleReadModell;

    public class EmailChangedController : ApiController
    {
        public IEnumerable<EmailChangedEventAndDate> Get()
        {
            return ShittyStaticReadModell.emailChanges.EventAndDate;
        }
    }
}