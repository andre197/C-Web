namespace BankAccountSystem.Models
{
    public interface IBankAccount
    {
        int Id { get; }

        string AccountNumber { get; }

        decimal Balance { get; }

        decimal fundsMultiplier { get; }

        int UserId { get;  set; }
        
        User User { get; }

        void adjustFunds();

        void Deposit(decimal amount);

        void Withdraw(decimal amount);
    }
}
