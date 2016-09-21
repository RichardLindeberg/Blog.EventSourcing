namespace Blog.EventSourcing.Domain.Commands
{
    using System;

    public interface IPersonCommand : ICommand
    {
        Guid PersonId { get; }
    }
}