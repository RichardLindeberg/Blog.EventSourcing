using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.EventSourcing.WebApi.Controller
{
    using System.Web.Http;

    using Blog.EventSourcing.SimpleReadModell;

    public class PersonController : ApiController
    {
        public IEnumerable<NameAndEmail> Get()
        {
            return ShittyStaticReadModell.People.Value.NameAndEmails;
        }
    }
}
