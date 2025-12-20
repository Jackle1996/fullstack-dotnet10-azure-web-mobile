namespace HealthTracking.Application.Dtos;

public record HealthRecordCreateDto
{
    public decimal WeightKg { get; set; }
    public decimal BodyFatPercentage { get; set; }
    public decimal BodyWaterPercentage { get; set; }
    public required string MedicationType { get; set; }
}
