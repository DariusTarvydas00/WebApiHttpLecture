using Microsoft.EntityFrameworkCore;
using WebApi.DataAccessLayer.Models;

namespace WebApi.DataAccessLayer
{

    public class MainDbContext : DbContext
    {

        public DbSet<Book> Books { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<User?> Users { get; set; }


        public MainDbContext(DbContextOptions<MainDbContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)

        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the User-Review relationship
            modelBuilder.Entity<User>()
                .HasMany(u => u.Reviews)
                .WithOne(r => r.User)
                .HasForeignKey(r => r.UserId)
                .IsRequired();

            // Configure the Book-Review relationship
            modelBuilder.Entity<Book>()
                .HasMany(b => b.Reviews)
                .WithOne(r => r.Book)
                .HasForeignKey(r => r.BookId)
                .IsRequired();

            // Additional configurations for User
            modelBuilder.Entity<User>()
                .ToTable("Users")
                .HasIndex(u => u.Email) // Assuming email should be unique and indexed
                .IsUnique();

            // Additional configurations for Book
            modelBuilder.Entity<Book>()
                .ToTable("Books");
        }
    }
}
