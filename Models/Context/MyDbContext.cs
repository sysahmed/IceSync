using Microsoft.EntityFrameworkCore;

namespace IceSync.Models.Context
{
    public class MyDbContext:DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options):base(options)
        {

        }
        public virtual DbSet<Workflow> Workflows { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=DEVOPS\\SQLEXPRESS;Database=MyDB;Integrated Security=true;");
            }
        }
    }
}
