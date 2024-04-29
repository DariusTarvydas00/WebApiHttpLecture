using Microsoft.EntityFrameworkCore;
using WebApi.DataAccessLayer.Models;

namespace WebApi.DataAccessLayer
{
    public class MainDbContext : DbContext
    {
        public MainDbContext(DbContextOptions<MainDbContext> options) : base(options)
        {
        }

        public DbSet<BookModel> Books { get; set; }
        public DbSet<ReviewModel> Reviews { get; set; }
        public DbSet<UserModel> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the User-Review relationship
            modelBuilder.Entity<UserModel>()
                .HasMany(u => u.Reviews)
                .WithOne(r => r.User)
                .HasForeignKey(r => r.UserId)
                .IsRequired();

            // Configure the Book-Review relationship
            modelBuilder.Entity<BookModel>()
                .HasMany(b => b.Reviews)
                .WithOne(r => r.Book)
                .HasForeignKey(r => r.BookId)
                .IsRequired();

            // Additional configurations for UserModel
            modelBuilder.Entity<UserModel>()
                .ToTable("Users")
                .HasIndex(u => u.Email) // Assuming email should be unique and indexed
                .IsUnique();

            // Additional configurations for BookModel
            modelBuilder.Entity<BookModel>()
                .ToTable("Books");
        }
    }
}
