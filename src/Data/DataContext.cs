using IDWM_TallerAPI.Src.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IDWM_TallerAPI.Src.Data
{
    public class DataContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<ProductType> ProductTypes { get; set; } = null!;
        public DbSet<Purchase> Purchases { get; set; } = null!;

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
    }
}