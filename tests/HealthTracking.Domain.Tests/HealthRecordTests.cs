namespace HealthTracking.Domain.Tests
{
    public class HealthRecordTests
    {
        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(-50)]
        public void WeightKg_ShouldThrow_WhenValueIsZeroOrNegative(decimal invalidWeight)
        {
            var record = new HealthRecord();

            Assert.Throws<ArgumentException>(() => record.WeightKg = invalidWeight);
        }

        [TestCase(1)]
        [TestCase(50)]
        [TestCase(120)]
        public void WeightKg_ShouldSet_WhenValueIsPositive(decimal validWeight)
        {
            var record = new HealthRecord();

            record.WeightKg = validWeight;

            Assert.That(record.WeightKg, Is.EqualTo(validWeight));
        }

        [TestCase(-1)]
        [TestCase(101)]
        [TestCase(150)]
        public void BodyFatPercentage_ShouldThrow_WhenOutsideRange(decimal invalidValue)
        {
            var record = new HealthRecord();

            Assert.Throws<ArgumentException>(() => record.BodyFatPercentage = invalidValue);
        }

        [TestCase(0)]
        [TestCase(25)]
        [TestCase(100)]
        public void BodyFatPercentage_ShouldSet_WhenWithinRange(decimal validValue)
        {
            var record = new HealthRecord();

            record.BodyFatPercentage = validValue;

            Assert.That(record.BodyFatPercentage, Is.EqualTo(validValue));
        }

        [TestCase(-1)]
        [TestCase(101)]
        [TestCase(200)]
        public void BodyWaterPercentage_ShouldThrow_WhenOutsideRange(decimal invalidValue)
        {
            var record = new HealthRecord();

            Assert.Throws<ArgumentException>(() => record.BodyWaterPercentage = invalidValue);
        }

        [TestCase(0)]
        [TestCase(40)]
        [TestCase(100)]
        public void BodyWaterPercentage_ShouldSet_WhenWithinRange(decimal validValue)
        {
            var record = new HealthRecord();

            record.BodyWaterPercentage = validValue;

            Assert.That(record.BodyWaterPercentage, Is.EqualTo(validValue));
        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void MedicationType_ShouldThrow_WhenNullOrEmpty(string? medicationType)
        {
            var record = new HealthRecord();

            Assert.Throws<ArgumentException>(() => record.MedicationType = medicationType!);
        }

        [TestCase("Vitamin D")]
        [TestCase("Aspirin")]
        [TestCase("Ibuprofen")]
        public void MedicationType_ShouldSet_WhenValid(string medicationType)
        {
            var record = new HealthRecord { MedicationType = medicationType };

            Assert.That(record.MedicationType, Is.EqualTo(medicationType));
        }

        [Test]
        public void RecordedAt_ShouldDefaultToUtcNow()
        {
            var before = DateTime.UtcNow;
            var record = new HealthRecord();
            var after = DateTime.UtcNow;

            Assert.That(record.RecordedAt, Is.InRange(before, after));
        }

        [Test]
        public void RecordedAt_ShouldAllowManualOverride()
        {
            var customDate = new DateTime(2020, 1, 1);
            var record = new HealthRecord { RecordedAt = customDate };

            Assert.That(record.RecordedAt, Is.EqualTo(customDate));
        }
    }
}
