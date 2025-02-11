namespace Solarnelle.Domain.Models.Tables
{
    public class User
    {
        public Guid Id { get; set; }

        public required string Email { get; set; }

        public required byte[] HashedPassword { get; set; }

        public required byte[] Salt { get; set; }

        public DateTime DateCreated { get; set; }

        public bool Enabled { get; set; }
    }
}
