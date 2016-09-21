using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.EventSourcing.UnitTest.GivenPerson
{
    using Blog.EventSourcing.Domain;
    using Blog.EventSourcing.Domain.Commands;
    using Blog.EventSourcing.Domain.Events;

    using NUnit.Framework;

    public class WhenCreated4DaysAgo
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
                                     DateTime.Now.AddDays(-4))
                             };
            _sut = new Person(events);
        }

        [Test]
        public void ShouldCorrectEmail()
        {
            var correctEmail = "richard.lindeberg@gmail.com";
            _sut.CorrectEmail(new CorrectPersonEmail(Guid.NewGuid(), _sut.Id, correctEmail));
            Assert.AreEqual(correctEmail, _sut.Email);
        }

        [Test]
        public void ShouldChangeEmail()
        {
            var correctEmail = "richard.lindeberg@gmail.com";
            _sut.ChangeEmail(new ChangePersonEmail(Guid.NewGuid(), _sut.Id, correctEmail));
            Assert.AreEqual(correctEmail, _sut.Email);
        }
    }
}
