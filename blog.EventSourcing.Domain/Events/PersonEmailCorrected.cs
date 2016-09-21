namespace Blog.EventSourcing.Domain.Events
{
    using System;

    public class PersonEmailCorrected : IPersonEmailChanged
    {
        public PersonEmailCorrected(Guid id, string email)
        {
            Id = id;
            Email = email;
        }

        public Guid Id { get; }

        public string Email { get; }
    }
}