using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.EventSourcing.CommandLineRunner
{
    using System.IO;

    using Blog.EventSourcing.Domain;

    using EventStore.ClientAPI.Common.Log;
    using EventStore.ClientAPI.Projections;
    using EventStore.ClientAPI.SystemData;

    public class ProjectionsBuilder
    {
        public  void AddAllProjections()
        {
            ProjectionsManager mgr = new ProjectionsManager(new ConsoleLogger(), IPEndPointFactory.DefaultHttp(), TimeSpan.FromSeconds(30));
            var projections = ListProjections(mgr);

            CreateProjections(projections, mgr);

            ListProjections(mgr);
        }

        private static List<ProjectionDetails> ListProjections(ProjectionsManager mgr)
        {
            
            var projections = mgr.ListAllAsync(new UserCredentials("admin", "changeit")).Result;
            
            foreach (var projectionDetailse in projections)
            {
                Console.WriteLine($"ProjectionName: {projectionDetailse.Name} ,status: {projectionDetailse.Status} ,statusReson: {projectionDetailse.StateReason}");
            }

            return projections;
        }

        private static void CreateProjections(List<ProjectionDetails> projections, ProjectionsManager mgr)
        {
            var files = Directory.GetFiles("Projections");
            //          mgr.CreateContinuousAsync("people", @"fromCategory('Person')
            //.whenAny(function(s, e) {
            //              linkTo('people', e);
            //          }); ", new UserCredentials("admin", "changeit")).Wait();

            foreach (var file in files)
            {
                Console.WriteLine(file);
                var fileInfo = new FileInfo(file);
                var projectionName = fileInfo.Name.Split('.')[0];

                if (projections.Any(t => t.Name == projectionName) == false)
                {
                    var content = File.ReadAllText(file);
                    Console.WriteLine(content);

                    mgr.CreateContinuousAsync(projectionName, content, new UserCredentials("admin", "changeit")).Wait();
                }
            }
        }
    }
}
