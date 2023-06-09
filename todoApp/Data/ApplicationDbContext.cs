using Microsoft.EntityFrameworkCore;
using todoApp.Models;

namespace todoApp.Data
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<todoModel> Todos { get; set; }
        public DbSet<User> Users { get; set; }


    }
}
