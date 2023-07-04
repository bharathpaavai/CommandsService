using CommandsService.Models;
using Microsoft.EntityFrameworkCore;

namespace CommandsService.Data
{
    public class AppDBContext :DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options ) : base(options) 
        { 
        
        }
        
        public DbSet<Platform> platforms { get; set; }
        public DbSet<Command> Commands { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder
                .Entity<Platform>()
                .HasMany(x => x.Commands)
                .WithOne(x => x.platform!)
                .HasForeignKey(x => x.PlatformId);

            modelBuilder
               .Entity<Command>()
               .HasOne(x => x.platform)
               .WithMany(x => x.Commands)
               .HasForeignKey(x => x.PlatformId);

        }


    }
}
