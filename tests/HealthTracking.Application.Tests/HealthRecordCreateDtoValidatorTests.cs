using FluentValidation.TestHelper;
using HealthTracking.Application.Dtos;
using HealthTracking.Application.Validators;

namespace HealthTracking.Application.Tests;


[TestFixture]
public class HealthRecordCreateDtoValidatorTests
{
    private HealthRecordCreateDtoValidator _validator;

    [SetUp]
    public void Setup()
    {
        _validator = new HealthRecordCreateDtoValidator();
    }

    [Test]
    public void Should_Have_Error_When_Weight_Is_Zero()
    {
        var dto = new HealthRecordCreateDto { WeightKg = 0, MedicationType = "Vitamin D" };
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.WeightKg);
    }

    [Test]
    public void Should_Have_Error_When_MedicationType_Is_Empty()
    {
        var dto = new HealthRecordCreateDto { MedicationType = "" };
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.MedicationType);
    }

    [Test]
    public void Should_Not_Have_Error_For_Valid_Dto()
    {
        var dto = new HealthRecordCreateDto
        {
            WeightKg = 80,
            BodyFatPercentage = 15,
            BodyWaterPercentage = 60,
            MedicationType = "Test"
        };

        var result = _validator.TestValidate(dto);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
