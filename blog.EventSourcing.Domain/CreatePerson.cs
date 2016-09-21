namespace Blog.EventSourcing.Domain
{
    using System;

    public class CreatePerson
    {
        public Guid CommandId { get; set; }

        public Guid PersonId { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }
    }
}
