using ETHTPS.Data.Core;
using ETHTPS.Data.Core.Models.Queries.Data.Requests;

namespace ETHTPS.Tests.Data
{
    [TestFixture]
    public class L2DataRequestModelTests
    {
        private L2DataRequestModel _model;

        [SetUp]
        public void Setup()
        {
            _model = new L2DataRequestModel();
        }

        [Test]
        public void AutoInterval_ReturnsCorrectInterval()
        {
            _model.StartDate = new DateTime(2022, 1, 1, 0, 0, 0);
            _model.EndDate = new DateTime(2022, 1, 1, 2, 30, 0);

            var interval = _model.AutoInterval;

            Assert.That(interval, Is.EqualTo(TimeInterval.OneHour));
        }

        [Test]
        public void Validate_StartAndEndDateNull_ReturnsInvalidResult()
        {
            var result = _model.Validate(new string[] { "provider1", "provider2" });

            Assert.IsFalse(result.IsValid);
            Assert.That(result.Reason, Is.EqualTo("Both StartDate and EndDate are null"));
        }

        [Test]
        public void Validate_StartDateLaterThanEndDate_ReturnsInvalidResult()
        {
            _model.StartDate = new DateTime(2022, 1, 2, 0, 0, 0);
            _model.EndDate = new DateTime(2022, 1, 1, 0, 0, 0);

            var result = _model.Validate(new string[] { "provider1", "provider2" });

            Assert.IsFalse(result.IsValid);
            Assert.That(result.Reason, Is.EqualTo("EndDate can't be earlier than StartDate"));
        }

        [Test]
        public void Validate_BucketSizeAndCustomBucketSizeSpecified_ReturnsInvalidResult()
        {
            _model.BucketOptions.BucketSize = TimeInterval.OneHour;
            _model.BucketOptions.CustomBucketSize = TimeSpan.FromMinutes(30);

            var result = _model.Validate(new string[] { "provider1", "provider2" });

            Assert.IsFalse(result.IsValid);
        }

        [Test]
        public void Validate_NoProvidersSpecified_ReturnsInvalidResult()
        {
            var result = _model.Validate(new string[] { "provider1", "provider2" });

            Assert.IsFalse(result.IsValid);
        }

        [Test]
        public void Validate_ProviderNotSupported_ReturnsInvalidResult()
        {
            _model.Provider = "unsupportedProvider";

            var result = _model.Validate(new string[] { "provider1", "provider2" });

            Assert.IsFalse(result.IsValid);
        }


        [Test]
        public void AllDistinctProviders_ReturnsDistinctProviders()
        {
            _model.Provider = "provider1";
            _model.Providers = new List<string> { "provider2", "provider1" };

            var providers = _model.AllDistinctProviders;

            Assert.That(providers.Count(), Is.EqualTo(2));
            Assert.IsTrue(providers.Contains("provider1"));
        }
    }
}
