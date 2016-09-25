namespace Blog.EventSourcing.UnitTest.GivenPerson
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Blog.EventSourcing.Domain;
    using Blog.EventSourcing.Domain.Commands;
    using Blog.EventSourcing.Events.Person;

    using NUnit.Framework;

    public class WhenCreated11DaysAgo
    {
        private Person _sut;

        [SetUp]
        public void Setup()
        {
            var events = new List<IPersonEvent>()
                             {
                                 new PersonCreated(
                                     Guid.NewGuid(),
                                     "Richard Lindeberg",
                                     "Richard.liiindeberg@gmailcom",
                                     DateTime.Now.AddDays(-11))
                             };
            _sut = new Person(events);
        }

        [Test]
        public void ShouldThrowErrorWhenCorrectingEmail()
        {
            var correctEmail = "richard.lindeberg@gmail.com";
            Assert.Throws<InvalidOperationException>(() => _sut.CorrectEmail(new CorrectPersonEmail(Guid.NewGuid(), _sut.Id, correctEmail)));
        }

        [Test]
        public void ShouldHaveNoUncommitedEventsWhenCrrecting()
        {
            var correctEmail = "richard.lindeberg@gmail.com";
            try
            {
                _sut.CorrectEmail(new CorrectPersonEmail(Guid.NewGuid(), _sut.Id, correctEmail));
            }
            catch (Exception)
            {
            }
            Assert.IsEmpty(_sut.UncommitEvents);
        }

        [Test]
        public void ShouldChangeEmail()
        {
            var correctEmail = "richard.lindeberg@gmail.com";
            _sut.ChangeEmail(new ChangePersonEmail(Guid.NewGuid(), _sut.Id, correctEmail));
            Assert.AreEqual(correctEmail, _sut.Email);
        }

        [Test]
        public void ShouldHaveHaveOneUncommitedEventsWhenChangingEmail()
        {
            var correctEmail = "richard.lindeberg@gmail.com";
            _sut.ChangeEmail(new ChangePersonEmail(Guid.NewGuid(), _sut.Id, correctEmail));
            Assert.AreEqual(1,_sut.UncommitEvents.Count());
        }
    }
}