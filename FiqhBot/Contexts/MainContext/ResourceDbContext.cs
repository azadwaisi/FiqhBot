using FiqhBot.Models.MainContextModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Options;

namespace FiqhBot.Contexts.MainContext
{
    public class ResourceDbContext : DbContext
    {
        public ResourceDbContext(DbContextOptions<ResourceDbContext> options)
        : base(options)
        {
            
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Chapter> Chapters { get; set; }
        public DbSet<Masaleh> MasalehList { get; set; }
        public DbSet<Chunk> Chunks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("vector");
            modelBuilder.Entity<Chunk>()
                        .Property(c => c.Embedding)
                        .HasColumnType("vector(1536)");
            modelBuilder.Entity<Chunk>()
            .Property(c => c.Tags)
            .HasColumnType("text[]");
            base.OnModelCreating(modelBuilder);
        }
    }
}
