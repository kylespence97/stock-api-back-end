using System;
using Microsoft.EntityFrameworkCore;

namespace TeamA.Data
{
    public class ProductManagementDb : DbContext
    {
        public DbSet<Models.StockDto> Stock { get; set; }
        public DbSet<Models.ResellHistoryDto> ResellHistory { get; set; }

        public ProductManagementDb(DbContextOptions<ProductManagementDb> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Models.StockDto>(x =>
            {
                x.Property(c => c.ID).IsRequired();
                x.Property(c => c.ProductID).IsRequired();
                x.Property(c => c.StockLevel).IsRequired();
                x.Property(c => c.ResellPrice).IsRequired();
            });

            modelBuilder.Entity<Models.ResellHistoryDto>(x =>
            {
                x.Property(c => c.ID).IsRequired();
                x.Property(c => c.ProductID).IsRequired();
                x.Property(c => c.ResellPrice).IsRequired();
                x.Property(c => c.TimeUpdated).IsRequired();
            });
        }
    }
}
