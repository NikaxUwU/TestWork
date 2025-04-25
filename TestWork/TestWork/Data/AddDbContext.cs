using Microsoft.EntityFrameworkCore;
using TestWork.Model;

namespace TestWork.Data
{
    public class AddDbContext : DbContext
    {
        public AddDbContext(DbContextOptions<AddDbContext> options)
            : base(options) { }
        
        public DbSet<Img> Images { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Img>().ToTable("Images");
        }
    }
}
