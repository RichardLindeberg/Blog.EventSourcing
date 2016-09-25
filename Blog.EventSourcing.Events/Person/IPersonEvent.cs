namespace Blog.EventSourcing.Events.Person
{
    using System;

    public interface IPersonEvent : IEvent
    {
        Guid Id { get; }
    }
}