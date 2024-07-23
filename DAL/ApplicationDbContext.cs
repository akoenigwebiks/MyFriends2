using Microsoft.EntityFrameworkCore;
using MyFriends2.DAL.Models;

namespace MyFriends2.DAL
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            // Database.EnsureCreated();

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Image> Images { get; set; }
    }
}
