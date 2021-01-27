using Microsoft.EntityFrameworkCore;

namespace toolShop.Models
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions options) : base(options) { }
        
        
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products {get;set;}
        public DbSet<UserCart> UserCarts {get;set;}
        public DbSet<UserPurchase> UserPurchases {get;set;}

    }
}