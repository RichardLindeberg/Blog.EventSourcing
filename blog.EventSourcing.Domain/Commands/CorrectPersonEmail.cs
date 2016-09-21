namespace Blog.EventSourcing.Domain.Commands
{
    using System;

    public class CorrectPersonEmail : IChangePersonEmail
    {
        public CorrectPersonEmail(Guid commandId, Guid personId, string email)
        {
            CommandId = commandId;
            PersonId = personId;
            Email = email;
        }

        public Guid CommandId { get; }

        public Guid PersonId { get; }

        public string Email { get; }
    }
}