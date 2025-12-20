using AutoMapper;
using HealthTracking.Application.Mapping;
using Microsoft.Extensions.Logging.Abstractions;

namespace HealthTracking.Application.Tests;

[TestFixture]
public class AutoMapperConfigurationTests
{
    private IMapper _mapper = null!;

    [SetUp]
    public void Setup()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new HealthRecordProfile());
        }, NullLoggerFactory.Instance);

        _mapper = config.CreateMapper();
    }

    [Test]
    public void AutoMapper_Configuration_IsValid()
    {
        _mapper.ConfigurationProvider.AssertConfigurationIsValid();
    }
}
