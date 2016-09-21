namespace Blog.EventSourcing.Domain.Commands
{
    using System;

    public class ChangePersonEmail : IChangePersonEmail
    {
        public ChangePersonEmail(Guid commandId, Guid personId, string email)
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