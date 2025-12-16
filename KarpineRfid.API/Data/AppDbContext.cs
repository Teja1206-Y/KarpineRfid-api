using KarpineRfid.API.Models;
using Microsoft.EntityFrameworkCore;

namespace KarpineRfid.API.Data
{
    public class AppDbContext : DbContext   // <-- MUST inherit from DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Tag> Tags { get; set; }
    }
}
