using Microsoft.EntityFrameworkCore;

namespace Domain
{
    public class ShopContext : DbContext
    {
        public ShopContext(DbContextOptions options)
        : base(options)
        {
            Database.EnsureCreated();
        }
      
        public virtual DbSet<Product> Products { get; set; }

        public virtual DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<OrderProducts>()
                .HasOne(x => x.Order)
                .WithMany(x => x.OrderProducts)
                .HasForeignKey(x => x.OrderId);

            modelBuilder.Entity<OrderProducts>()
              .HasOne(x => x.Product)
              .WithMany(x => x.OrderProducts)
              .HasForeignKey(x => x.ProductId);

            modelBuilder.Entity<OrderProducts>()
                .HasKey(x => new { x.OrderId, x.ProductId });

            modelBuilder.Entity<Product>()
                .HasData(new Product
                {
                    Id = 1, Name = "Apple", Type = "Fruit"
                }, new Product
                {
                    Id = 2, Name = "Orange", Type = "Fruit"
                }, new Product
                {
                    Id = 3, Name = "Apricot", Type = "Fruit"
                }, new Product
                {
                    Id = 4, Name = "Mandarin", Type = "Fruit"
                }, new Product
                {
                    Id = 5, Name = "Banana", Type = "Fruit"
                });
        }
    }
}
