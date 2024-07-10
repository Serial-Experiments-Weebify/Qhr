using Microsoft.EntityFrameworkCore;

namespace Qhr.Server;

using Server.Models;

public class QhrContext : DbContext
{
    public DbSet<Job> Jobs { get; set; }

    public QhrContext(DbContextOptions<QhrContext> opts) : base(opts) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Job>().ToTable("Job");
    }
}