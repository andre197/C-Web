namespace BankAccountSystem.Data
{
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class BankDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<SavingsAccount> BankAccounts { get; set; }

        public DbSet<CheckingAccount> CheckingAccounts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseSqlServer(@"Server=LAPTOP-22QSF26P\SQLEXPRESS;Database=BankAccountTestDb;Integrated Security=True");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<BankAccount>()
                .HasOne(ba => ba.User)
                .WithMany(u => u.BankAccounts)
                .HasForeignKey(ba => ba.UserId);
        }
    }
}
