namespace TheRestExercisesIncludedInTheLab.Data
{
    using Microsoft.EntityFrameworkCore;

    public class ShopContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Salesman> Salesmans { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Item> Items { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseSqlServer(@"Server=LAPTOP-22QSF26P\SQLEXPRESS;Database=TestDb;Integrated Security=True");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .Entity<Customer>()
                .HasOne(c => c.Salesman)
                .WithMany(s => s.Customers)
                .HasForeignKey(c => c.SalesmanId);

            builder
                .Entity<Order>()
                .HasOne(o => o.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.CustomerId);

            builder
                .Entity<Review>()
                .HasOne(r => r.Customer)
                .WithMany(c => c.Reviews)
                .HasForeignKey(r => r.CustomerId);

            builder
                .Entity<ItemOrder>()
                .HasKey(io => new {io.ItemId, io.OrderId});

            builder
                .Entity<ItemOrder>()
                .HasOne(io => io.Item)
                .WithMany(i => i.Orders)
                .HasForeignKey(io => io.ItemId);

            builder
                .Entity<ItemOrder>()
                .HasOne(io => io.Order)
                .WithMany(o => o.Items)
                .HasForeignKey(io => io.OrderId);

            builder
                .Entity<Review>()
                .HasOne(r => r.Item)
                .WithMany(i => i.Reviews)
                .HasForeignKey(r => r.ItemId);
        }
    }
}
