using Microsoft.EntityFrameworkCore;
using ParadeDB.EFCore.Shared;

namespace ParadeDB.EFCore.Rag.Data;

public class AppDbContext : DbContext
{
    public DbSet<MockItem> MockItems { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MockItem>().Property(e => e.Id).ValueGeneratedNever();
    }
}
