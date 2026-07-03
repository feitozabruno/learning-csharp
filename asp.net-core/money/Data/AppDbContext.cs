using Microsoft.EntityFrameworkCore;
using Money.Models;

namespace Money.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<Transaction> Transactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Transaction>()
            .Property(t => t.Value)
            .HasPrecision(18, 2); // 18 dígitos no total, 2 depois da vírgula
    }
}