namespace Solarnelle.Domain.Models.Tables
{
    public class User
    {
        public Guid Id { get; set; }

        public required string Email { get; set; }

        public required DateTime DateCreated { get; set; }

        public bool Enabled { get; set; }
    }
}
