using FluentValidation;
using HealthTracking.Application.Dtos;

namespace HealthTracking.Application.Validators;

public class HealthRecordCreateDtoValidator : AbstractValidator<HealthRecordCreateDto>
{
    public HealthRecordCreateDtoValidator()
    {
        RuleFor(x => x.WeightKg).GreaterThan(0);
        RuleFor(x => x.BodyFatPercentage).InclusiveBetween(0, 100);
        RuleFor(x => x.BodyWaterPercentage).InclusiveBetween(0, 100);
        RuleFor(x => x.MedicationType).NotEmpty();
    }
}
