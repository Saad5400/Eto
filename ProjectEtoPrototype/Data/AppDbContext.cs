using Microsoft.EntityFrameworkCore;
using ProjectEtoPrototype.Models;

namespace ProjectEtoPrototype.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<DailyTask> DailyTasks { get; set; }
        public DbSet<Preference> Preferences { get; set; }
    }
}
