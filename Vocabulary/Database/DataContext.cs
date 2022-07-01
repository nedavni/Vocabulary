using System.IO;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Vocabulary.Database.Entities;

namespace Vocabulary.Database
{
    internal class DataContext : DbContext
    {
        public DataContext()
        {
        }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Word> Words { get; set; }

        public DbSet<Meaning> Meanings { get; set; }

        public DbSet<Text> Texts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString =
                $@"Data Source=(LocalDb)\MSSQLLocalDB;Initial Catalog=Vocabulary;AttachDBFilename={Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "VocabularyDB.mdf")}";
            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Word>().Navigation(w => w.Meanings).AutoInclude();

            modelBuilder.Entity<Word>()
                .HasMany(w => w.Meanings)
                .WithOne(m => m.Word)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}
