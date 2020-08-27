using Cacanta.WebUI.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Cacanta.WebUI.Repository.Concrete.EntityFramework
{
    public class CacantaContext : DbContext
    {
 
        public CacantaContext(DbContextOptions<CacantaContext> options) 
            : base(options)
        {

        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductCategory>()
                .HasKey(pk  => new { pk.ProductId, pk.CategoryId });
        }

    }
}
