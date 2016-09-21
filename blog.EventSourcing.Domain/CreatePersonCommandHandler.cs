namespace Blog.EventSourcing.Domain
{
    using Blog.EventSourcing.Domain.Commands;

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

        public void Execute(ChangePersonName command)
        {
            _repository.ActOn(command.PersonId, command.CommandId, person => person.ChangePersonName(command));
        }

        public void Execute(CorrectPersonEmail command)
        {
            _repository.ActOn(command.PersonId, command.CommandId, person => person.CorrectEmail(command));
        }

        public void Execute(ChangePersonEmail command)
        {
            _repository.ActOn(command.PersonId, command.CommandId, person => person.ChangeEmail(command));
        }
    }
}