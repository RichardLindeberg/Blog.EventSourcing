namespace Blog.EventSourcing.Domain
{
    using System.Threading.Tasks;

    using Blog.EventSourcing.Domain.Commands;

    using EventStore.ClientAPI;

    public class CreatePersonCommandHandler
    {
        private readonly PersonRepository _repository;

        public CreatePersonCommandHandler(PersonRepository repository)
        {
            _repository = repository;
        }

        public Task<WriteResult> Execute(CreatePerson command)
        {
            return _repository.ActOn(command.PersonId, command.CommandId, person => person.Create(command));
        }

        public Task<WriteResult> Execute(ChangePersonName command)
        {
            return _repository.ActOn(command.PersonId, command.CommandId, person => person.ChangePersonName(command));
        }

        public Task<WriteResult> Execute(CorrectPersonEmail command)
        {
            return _repository.ActOn(command.PersonId, command.CommandId, person => person.CorrectEmail(command));
        }

        public Task<WriteResult> Execute(ChangePersonEmail command)
        {
            return _repository.ActOn(command.PersonId, command.CommandId, person => person.ChangeEmail(command));
        }
    }
}