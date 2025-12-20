using HealthTracking.Api.Data;
using HealthTracking.Application.Dtos;
using HealthTracking.Domain;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;
using Testcontainers.MsSql;


namespace HealthTracking.Api.Tests
{
    [TestFixture]
    public class HealthRecordsControllerTests
    {
        private MsSqlContainer _container;
        private WebApplicationFactory<Program> _factory;

        [SetUp]
        public async Task SetUp()
        {
            _container = new MsSqlBuilder()
               .WithPassword("yourStrong(!)Password")
               .Build();

            await _container.StartAsync();

            _factory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        // Replace DbContext with InMemory for testing
                        var descriptor = services.SingleOrDefault(
                            d => d.ServiceType == typeof(DbContextOptions<HealthTrackingDbContext>));
                        if (descriptor != null) services.Remove(descriptor);

                        services.AddDbContext<HealthTrackingDbContext>(options =>
                        {
                            options.UseSqlServer(_container.GetConnectionString());
                        });
                    });
                });

        }

        [TestCase("2025-01-01", "2025-01-01", 1)]
        [TestCase("2025-01-01", "2025-06-30", 2)]
        public async Task GetHealthRecords_ReturnsRecordsWithinDateRange(string start, string end, int expected)
        {
            var client = _factory.CreateClient();

            // Seed test data
            using (var scope = _factory.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<HealthTrackingDbContext>();
                db.HealthRecords.AddRange(
                    new HealthRecord { RecordedAt = new DateTime(2025, 1, 1), MedicationType = "A", WeightKg = 50.5m },
                    new HealthRecord { RecordedAt = new DateTime(2025, 6, 1), MedicationType = "B", WeightKg = 50.5m },
                    new HealthRecord { RecordedAt = new DateTime(2025, 12, 1), MedicationType = "C", WeightKg = 50.5m }
                );
                db.SaveChanges();
            }
            var startDate = DateTime.Parse(start);
            var endDate = DateTime.Parse(end);

            var response = await client.GetAsync($"/api/HealthRecords?startDate={startDate:o}&endDate={endDate:o}");
            response.EnsureSuccessStatusCode();

            var records = await response.Content.ReadFromJsonAsync<List<HealthRecord>>();
            Assert.That(records, Is.Not.Null);
            Assert.That(records.Count, Is.EqualTo(expected));
        }

        [Test]
        public async Task PostHealthRecord_CreatesRecord_ReturnsCreated()
        {
            var client = _factory.CreateClient();

            var dto = new HealthRecordCreateDto
            {
                MedicationType = "Vitamin D",
                WeightKg = 72.5m,
                BodyFatPercentage = 18.2m,
                BodyWaterPercentage = 55.0m
            };

            var response = await client.PostAsJsonAsync("/api/HealthRecords", dto);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));

            // Verify record persisted
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<HealthTrackingDbContext>();
            var record = db.HealthRecords.SingleOrDefault(r => r.MedicationType == "Vitamin D");
            Assert.That(record, Is.Not.Null);
            Assert.That(record.WeightKg, Is.EqualTo(dto.WeightKg));
            Assert.That(record.MedicationType, Is.EqualTo(dto.MedicationType));
            Assert.That(record.BodyFatPercentage, Is.EqualTo(dto.BodyFatPercentage));
            Assert.That(record.BodyWaterPercentage, Is.EqualTo(dto.BodyWaterPercentage));
        }

        [TearDown]
        public async Task DisposeAsync()
        {
            await _container.DisposeAsync();
            await _factory.DisposeAsync();
        }
    }
}
