namespace BankAccountSystem.Models
{
    public class CheckingAccount : BankAccount
    {
        public override void adjustFunds()
        {
            this.Balance += ((this.Balance * fundsMultiplier) * -1);
        }
    }
}
