namespace BankAccountSystem.Models
{
    public abstract class BankAccount : IBankAccount
    {
        public int Id { get; protected set; }

        public string AccountNumber { get; protected set; }

        public decimal Balance { get; protected set; }

        public decimal fundsMultiplier { get; protected set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public abstract void adjustFunds();

        public void Deposit(decimal amount)
        {
            this.Balance += amount;
        }

        public void Withdraw(decimal amount)
        {
            this.Balance -= amount;
        }
    }
}
