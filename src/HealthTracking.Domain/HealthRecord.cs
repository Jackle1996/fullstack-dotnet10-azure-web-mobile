namespace HealthTracking.Domain;

public class HealthRecord
{
    public int Id { get; set; }

    private decimal _weightKg;
    public decimal WeightKg
    {
        get => _weightKg;
        set
        {
            if (value <= 0)
                throw new ArgumentException("Weight must be greater than zero.");
            _weightKg = value;
        }
    }

    private decimal _bodyFatPercentage;
    public decimal BodyFatPercentage
    {
        get => _bodyFatPercentage;
        set
        {
            if (value < 0 || value > 100)
                throw new ArgumentException("Body fat percentage must be between 0 and 100.");
            _bodyFatPercentage = value;
        }
    }

    private decimal _bodyWaterPercentage;
    public decimal BodyWaterPercentage
    {
        get => _bodyWaterPercentage;
        set
        {
            if (value < 0 || value > 100)
                throw new ArgumentException("Body water percentage must be between 0 and 100.");
            _bodyWaterPercentage = value;
        }
    }

    private string _medicationType = string.Empty;
    public string MedicationType
    {
        get => _medicationType;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Medication type cannot be empty.");

            _medicationType = value;
        }
    }

    public DateTime RecordedAt { get; set; } = DateTime.UtcNow;
}
