namespace Blog.EventSourcing.Domain
{
    public class CreatePersonCommandHandler
    {
        private readonly PersonRepository _repository;

        public CreatePersonCommandHandler(PersonRepository repository)
        {
            _repository = repository;
        }

        public void Execute(CreatePerson command)
        {
            _repository.ActOn(command.PersonId, command.CommandId, person => person.Create(command));
        }
    }
}