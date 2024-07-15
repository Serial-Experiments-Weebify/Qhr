namespace Qhr.Server;

using Microsoft.EntityFrameworkCore;
using Server.Models;

public class QhrContext(DbContextOptions<QhrContext> opts) : DbContext(opts)
{
    public DbSet<Job> Jobs { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Job>().ToTable("Job");
        modelBuilder.Entity<User>().ToTable("User");
    }
}