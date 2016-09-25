namespace Blog.EventSourcing.Events.Person
{
    using System;

    public class PersonEmailChanged : IPersonEmailChanged
    {
        public PersonEmailChanged(Guid id, string email)
        {
            Id = id;
            Email = email;
        }

        public Guid Id { get; }

        public string Email { get; }
    }
}