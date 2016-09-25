namespace Blog.EventSourcing.UnitTest.GivenSubscription
{
    using System;
    using Blog.EventSourcing.Domain;
    using Blog.EventSourcing.Domain.Commands;
    using EventStore.Projections.Core.Utils;
    using NUnit.Framework;

    [TestFixture]
    public class WhenSubscribing
    {
        private PersonRepository _repository;

        [SetUp]
        public void SetUp()
        {
            _repository = new PersonRepository(new StoreFactory());
        }

        [Test]
        public void ShouldEventsApear()
        {
            var store = new StoreFactory().GetStore();

            var s = store.SubscribeToStreamAsync(
                "People",
                true,
                (subscription, @event) =>
                    Console.WriteLine($"Incomming event {@event.Event.Data.FromUtf8()} for {subscription.StreamId}"));

            var read = store.ReadStreamEventsForwardAsync("People", 0, 4096, true).Result;
            foreach (var item in read.Events)
            {
                Console.WriteLine("Eventtypes: " + item.Event.EventType);
            }
            
            var cmd = new CreatePerson(Guid.NewGuid(), Guid.NewGuid(), "Name", "EMail");
            _repository.ActOn(cmd.PersonId, cmd.CommandId, person => person.Create(cmd)).Wait();
            s.Wait();
        }
    }
}
