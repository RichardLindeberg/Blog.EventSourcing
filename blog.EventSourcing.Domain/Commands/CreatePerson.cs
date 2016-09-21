namespace Blog.EventSourcing.Domain.Commands
{
    using System;

    public class CreatePerson : IPersonCommand
    {
        public CreatePerson(Guid commandId, Guid personId, string name, string email)
        {
            CommandId = commandId;
            PersonId = personId;
            Name = name;
            Email = email;
        }

        public Guid CommandId { get; }

        public Guid PersonId { get; }

        public string Name { get; }

        public string Email { get;}
    }
}
