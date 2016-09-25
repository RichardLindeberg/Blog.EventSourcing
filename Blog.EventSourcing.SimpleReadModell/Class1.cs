using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.EventSourcing.SimpleReadModell
{
    public class NameAndEmail
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public Guid PersonId { get; set; }
    }
}
