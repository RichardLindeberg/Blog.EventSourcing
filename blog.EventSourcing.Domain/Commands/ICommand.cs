namespace Blog.EventSourcing.Domain.Commands
{
    using System;

    public interface ICommand
    {
        Guid CommandId { get; }
    }
}