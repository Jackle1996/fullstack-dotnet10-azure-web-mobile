namespace HealthTracking.WebApi.Models;

public class HealthRecord
{
    public int Id { get; set; }

    public decimal WeightKg { get; set; }

    public decimal BodyFatPercentage { get; set; }

    public decimal BodyWaterPercentage { get; set; }

    public required string MedicationType { get; set; }

    public DateTime RecordedAt { get; set; } = DateTime.UtcNow;
}
