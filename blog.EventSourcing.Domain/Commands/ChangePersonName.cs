namespace Blog.EventSourcing.Domain.Commands
{
    using System;

    public class ChangePersonName : IPersonCommand
    {
        public ChangePersonName(Guid commandId, Guid personId, string name)
        {
            CommandId = commandId;
            PersonId = personId;
            Name = name;
        }

        public Guid CommandId { get; }

        public Guid PersonId { get; }

        public string Name { get;  }
    }
}