namespace Blog.EventSourcing.Events.Person
{
    public interface IPersonEmailChanged : IPersonEvent
    {
        string Email { get; }
    }
}