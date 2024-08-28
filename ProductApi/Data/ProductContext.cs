using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProductApi.Models;
using ProductAPI.Models;


namespace ProductAPI.Data
{
    public class ProductContext : IdentityDbContext<ApplicationUser>
    {
        public ProductContext(DbContextOptions<ProductContext> options)
            : base(options)
        {
        }
        public DbSet<ProductModel> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ProductModel>()
                .HasIndex(p => new { p.ManufactureEmail, p.ProduceDate })
                .IsUnique();
        }
    }
}
