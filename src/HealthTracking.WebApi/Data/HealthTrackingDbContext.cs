using HealthTracking.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace HealthTracking.WebApi.Data;

public class HealthTrackingDbContext(DbContextOptions<HealthTrackingDbContext> options) : DbContext(options)
{
    public DbSet<HealthRecord> HealthRecords => Set<HealthRecord>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<HealthRecord>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.WeightKg)
                .HasPrecision(5, 2)
                .IsRequired();
            
            entity.Property(e => e.BodyFatPercentage)
                .HasPrecision(5, 2)
                .IsRequired();
            
            entity.Property(e => e.BodyWaterPercentage)
                .HasPrecision(5, 2)
                .IsRequired();
            
            entity.Property(e => e.MedicationType)
                .HasMaxLength(100)
                .IsRequired();
            
            entity.Property(e => e.RecordedAt)
                .IsRequired();

            entity.HasIndex(e => e.RecordedAt).IsDescending();
        });
    }
}
