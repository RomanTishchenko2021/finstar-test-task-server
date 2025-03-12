using Microsoft.EntityFrameworkCore;
using FinstarTestTask.Entities;

namespace FinstarTestTask.Infrastructure;

public class FinstarTestTaskDbContext : DbContext
{
    public DbSet<SomeObject> SomeObjects { get; set; }

    public FinstarTestTaskDbContext(DbContextOptions<FinstarTestTaskDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<SomeObject>(entity =>
        {
            entity.HasKey(e => e.Number);
            entity.Property(e => e.Code).IsRequired();
            entity.Property(e => e.Value).HasMaxLength(100);
        });
    }
}