namespace BankAccountSystem.Models
{
    using System.Collections.Generic;

    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public ICollection<BankAccount> BankAccounts { get; set; } = new HashSet<BankAccount>();
    }
}
