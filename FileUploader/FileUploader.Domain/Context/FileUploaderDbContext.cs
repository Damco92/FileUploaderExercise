using FileUploader.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace FileUploader.Domain.Context
{
    public class FileUploaderDbContext : DbContext
    {
        public FileUploaderDbContext() {}

        public FileUploaderDbContext(DbContextOptions<FileUploaderDbContext> options) : base(options) { }

        public virtual DbSet<File> Files { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<File>(entity =>
            {
                entity.Property(e => e.FileId).IsRequired().ValueGeneratedOnAdd();

                entity.Property(e => e.FileName).IsRequired().HasMaxLength(100);

                entity.Property(e => e.FileData).IsRequired().HasMaxLength(8000);

                entity.Property(e => e.Created).IsRequired();
            });
        }
    }
}
