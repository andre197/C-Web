namespace BankAccountSystem.Models
{
    public class SavingsAccount : BankAccount
    {
        public override void adjustFunds()
        {
            this.Balance += this.Balance * fundsMultiplier;
        }
    }
}
