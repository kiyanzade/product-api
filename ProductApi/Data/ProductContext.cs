using Microsoft.EntityFrameworkCore;
using ProductAPI.Models;


namespace ProductAPI.Data
{
    public class ProductContext : DbContext
    {
        public ProductContext(DbContextOptions<ProductContext> options)
            : base(options)
        {
        }
        public DbSet<ProductModel> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductModel>()
                .HasIndex(p => new { p.ManufactureEmail, p.ProduceDate })
                .IsUnique();
        }
    }
}
