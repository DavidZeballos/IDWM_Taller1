using IDWM_TallerAPI.Src.Models;
using Microsoft.EntityFrameworkCore;

namespace IDWM_TallerAPI.Src.Data
{
    public class DataContext : DbContext
    {
        public DbSet<User> Users { get; set;} = null!;

        public DbSet<Role> Roles { get; set;} = null!;

        public DbSet<Product> Products { get; set;} = null!;

        public DbSet<ProductType> ProductTypes { get; set;} = null!;

        public DbSet<Purchase> Purchases { get; set;} = null!;

        public DataContext(DbContextOptions options) : base(options)
        {

        }
    }
}