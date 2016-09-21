namespace Blog.EventSourcing.Domain.Events
{
    public interface IPersonEmailChanged : IPersonEvent
    {
        string Email { get; }
    }
}