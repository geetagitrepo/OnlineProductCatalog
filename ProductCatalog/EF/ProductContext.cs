using Microsoft.EntityFrameworkCore;
using ProductCatalog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductCatalog.EF
{
    public interface IContext
    {
        DbSet<Product> Products { get; set; }
    }

    public class ProductContext : DbContext,IContext
    {
        public ProductContext()
        {

        }

        public ProductContext(DbContextOptions<ProductContext> options)
        : base(options)
        {
        }

        public virtual DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            {
               modelBuilder.Entity<Product>()
                    .HasIndex(p => p.Code).IsUnique();

                modelBuilder.Entity<Product>()
               .Property(p => p.RowVersion).IsConcurrencyToken();

            }
        }
    }
}

