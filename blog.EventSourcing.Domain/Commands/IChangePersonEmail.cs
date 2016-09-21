namespace Blog.EventSourcing.Domain.Commands
{
    public interface IChangePersonEmail : IPersonCommand
    {
        string Email { get; }
    }
}