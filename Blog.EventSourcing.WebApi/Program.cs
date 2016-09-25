using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.EventSourcing.WebApi
{
    using System.Net.Http;

    using Microsoft.Owin.Hosting;

    class Program
    {
        static void Main(string[] args)
        {
            string baseAddress = "http://localhost:9000/";

            // Start OWIN host 
            using (WebApp.Start<Startup>(url: baseAddress))
            {
                Console.WriteLine("Press anykey to shutdown");
                Console.ReadLine();
            }

            
        }
    }
}
