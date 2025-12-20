using HealthTracking.Domain;

namespace HealthTracking.Api.Models
{
    public static class HealthRecordExtensions
    {
        public static HealthRecordDto ToDto(this HealthRecord record) =>
            new HealthRecordDto
            {
                WeightKg = record.WeightKg,
                BodyFatPercentage = record.BodyFatPercentage,
                BodyWaterPercentage = record.BodyWaterPercentage,
                MedicationType = record.MedicationType
            };

        public static HealthRecord ToEntity(this HealthRecordDto dto) =>
            new HealthRecord
            {
                WeightKg = dto.WeightKg,
                BodyFatPercentage = dto.BodyFatPercentage,
                BodyWaterPercentage = dto.BodyWaterPercentage,
                MedicationType = dto.MedicationType
            };

    }
}
