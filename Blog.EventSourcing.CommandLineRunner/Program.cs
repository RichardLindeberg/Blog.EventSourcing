using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.EventSourcing.CommandLineRunner
{
    using Blog.EventSourcing.Domain;
    using Blog.EventSourcing.Domain.Commands;

    using EventStore.ClientAPI;
    using EventStore.ClientAPI.Common.Log;
    using EventStore.ClientAPI.Projections;
    using EventStore.ClientAPI.SystemData;
    using EventStore.Common.Utils;

    class Program
    {
        static void Main(string[] args)
        {
            
            var store = new StoreFactory().GetStore();
            var pr = new ProjectionsBuilder();
            pr.AddAllProjections();

            var s = store.SubscribeToStreamFrom(
                "people", 
                0, 
                CatchUpSubscriptionSettings.Default,
               (subscription, @event) => Console.WriteLine($"Incomming event {@event.Event.ToIPersonEvent().ToJson()} for {subscription.StreamId}"),
               subscription => Console.WriteLine("Started catchup"),
               (subscription, reason, arg3) => Console.WriteLine("Dropped subscription"));

            var cmdHanlder = new CreatePersonCommandHandler(new PersonRepository(new StoreFactory()));
            var cmd = new CreatePerson(Guid.NewGuid(), Guid.NewGuid(), "Name", "EMail");

            cmdHanlder.Execute(cmd).Wait();
            var changeEmailCmd = new ChangePersonEmail(Guid.NewGuid(), cmd.PersonId, "New email");
            cmdHanlder.Execute(changeEmailCmd).Wait();
            
            var correctEmailCmd = new CorrectPersonEmail(Guid.NewGuid(), cmd.PersonId, "CorrectedEMail");
            cmdHanlder.Execute(correctEmailCmd).Wait();

            Console.WriteLine("Waiting");
            Console.ReadKey();
        }
    }
}
