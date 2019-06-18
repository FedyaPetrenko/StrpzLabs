using System;
using System.Collections.Generic;
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

            var products = new List<Product>();
            //for (int i = 1; i < 100000; i++)
            //{
            //    products.Add(new Product
            //    {
            //        Id = i,
            //        Name = $"Apple{i}",
            //        Type = $"Fruit{i}"
            //    });
            //}

            modelBuilder.Entity<Product>()
                .HasData(products);

            var orders = new List<Order>();
            for (int i = 1; i < 20000; i++)
            {
                orders.Add(new Order
                {
                    Id = i,
                    Number = new Random().Next(Int32.MinValue, Int32.MaxValue),
                    StatusValue = new Random().Next(0, 1)
                });
            }

            modelBuilder.Entity<Order>()
                .HasData(orders);
        }
    }
}
