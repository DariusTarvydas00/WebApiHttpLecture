using Microsoft.EntityFrameworkCore;
using WebApi.DataAccessLayer.Models;

namespace WebApi.DataAccessLayer;

public class MainDbContext : DbContext
{
    public DbSet<UserModel> Users { get; set; }

    public MainDbContext(DbContextOptions<MainDbContext> options) : base(options)
    {

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
}