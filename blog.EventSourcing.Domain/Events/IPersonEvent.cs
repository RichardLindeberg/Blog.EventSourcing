namespace Blog.EventSourcing.Domain.Events
{
    using System;

    public interface IPersonEvent : IEvent
    {
        Guid Id { get; }
    }
}