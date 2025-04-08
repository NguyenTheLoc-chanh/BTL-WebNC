using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BTL_WEBNC.Models 
{
    public class AppDBContext : IdentityDbContext<User>
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            base.OnConfiguring(builder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Code tích hợp Identity
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var tableName = entityType.GetTableName();
                if (tableName.StartsWith("AspNet"))
                {
                    entityType.SetTableName(tableName.Substring(6));
                }
            }

        }
        // Ví dụ khai báo
        //public DbSet<Contact> Contacts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Seller> Sellers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductSize>  ProductSizes{ get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<CartModel> Carts { get; set; }
        public DbSet<CartDetails> CartDetails { get; set; }

        public DbSet<Address> AddressUser { get; set; }

        public DbSet<Orders> Orders { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<Payment> Payments { get; set; }

    }
}