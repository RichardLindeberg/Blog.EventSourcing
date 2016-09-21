namespace Blog.EventSourcing.Domain
{
    using System;

    public interface IPersonEvent : IEvent
    {
        Guid Id { get; }
    }
}