namespace HealthTracking.Application.Dtos;

public record HealthRecordReadDto
{
    public int Id { get; set; }
    public decimal WeightKg { get; set; }
    public decimal BodyFatPercentage { get; set; }
    public decimal BodyWaterPercentage { get; set; }
    public string MedicationType { get; set; } = default!;
    public DateTime RecordedAt { get; set; }
}
