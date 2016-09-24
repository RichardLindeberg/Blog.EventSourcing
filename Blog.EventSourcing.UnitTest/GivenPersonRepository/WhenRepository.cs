using System;

namespace Blog.EventSourcing.UnitTest.GivenPersonRepository
{
    using Blog.EventSourcing.Domain;
    using Blog.EventSourcing.Domain.Commands;

    using NUnit.Framework;

    [TestFixture]
    public class WhenRepository
    {
        private PersonRepository _repository;

        [SetUp]
        public void Setup()
        {
            _repository = new PersonRepository(new StoreFactory());
        }

        [Test]
        public void ShouldAddWithoutError()
        {
            var cmd = new CreatePerson(
                Guid.NewGuid(), 
                Guid.NewGuid(), 
                "Rille", 
                "richard.lindeberg@gmail.com");

            Assert.DoesNotThrow(
                () => _repository.ActOn(cmd.PersonId, cmd.CommandId, person => person.Create(cmd)).Wait());
        }

        [Test]
        public void ShouldAddTwoWithoutError()
        {
            var cmd = new CreatePerson(Guid.NewGuid(), Guid.NewGuid(), "Rille", "richard.lindeberg@gmail.com");
            Assert.DoesNotThrow(
                () => _repository.ActOn(cmd.PersonId, cmd.CommandId, person => person.Create(cmd)).Wait());
            var cmd2 = new ChangePersonName(Guid.NewGuid(), cmd.PersonId, "Richard Lindeberg");
            Assert.DoesNotThrow(
                () => _repository.ActOn(cmd2.PersonId, cmd.CommandId, person => person.ChangePersonName(cmd2)).Wait());
        }
    }
}
