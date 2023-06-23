using ETHTPS.Data.Core.Models.LiveData.Triggers;
using ETHTPS.Services.LiveData;

namespace ETHTPS.Tests.ServiceTests
{
    [TestFixture]
    [Category("Data")]
    [Category("BusinessLogic")]
    public class LatestEntryAggregatorTests
    {
        private sealed class TestObject
        {
            public int Id { get; set; }
            public string? Name { get; set; }
        }

        private LatestEntryAggregator<string, L2DataUpdateModel> _aggregator1 = new();
        private LatestEntryAggregator<string, L2DataUpdateModel> _aggregator2 = new();

        [SetUp]
        public void Setup()
        {
            _aggregator1 = new LatestEntryAggregator<string, L2DataUpdateModel>();
            _aggregator2 = new LatestEntryAggregator<string, L2DataUpdateModel>();
        }

        [Test]
        public void Diff_Should_Return_New_Entries()
        {
            // Arrange
            var entry1 = new L2DataUpdateModel { Provider = "Provider1" };
            var entry2 = new L2DataUpdateModel { Provider = "Provider2" };
            _aggregator1.Push(entry1.Provider, entry1);
            _aggregator2.Push(entry1.Provider, entry1);
            _aggregator2.Push(entry2.Provider, entry2);

            // Act
            var diff = _aggregator1.Diff(_aggregator2, (a, b) => a.Provider != b.Provider).ToList();

            // Assert
            Assert.That(diff.Count, Is.EqualTo(1));
            Assert.That(diff.First(), Is.SameAs(entry2));
        }

        [Test]
        public void Diff_Should_Return_Updated_Entries()
        {
            // Arrange
            var entry1 = new L2DataUpdateModel { Provider = "Provider1" };
            var updatedEntry1 = new L2DataUpdateModel { Provider = "Provider1", BlockNumber = 1 };
            _aggregator1.Push(entry1.Provider, entry1);
            _aggregator2.Push(updatedEntry1.Provider, updatedEntry1);

            // Act
            var diff = _aggregator1.Diff(_aggregator2, (a, b) => a.BlockNumber != b.BlockNumber).ToList();

            // Assert
            Assert.That(diff.Count, Is.EqualTo(1));
            Assert.That(diff.First(), Is.SameAs(updatedEntry1));
        }

        [Test]
        public void Constructor_WithSourceAndKeySelector_ShouldInitializeDictionary()
        {
            var source = new List<TestObject>
            {
                new TestObject { Id = 1, Name = "Object1" },
                new TestObject { Id = 2, Name = "Object2" },
                new TestObject { Id = 3, Name = "Object3" },
            };
            var aggregator = new LatestEntryAggregator<int, TestObject>(source, obj => obj.Id);

            Assert.That(aggregator.Count(), Is.EqualTo(3));
        }

        [Test]
        public void Push_ShouldOverwriteExistingValues()
        {
            var aggregator = new LatestEntryAggregator<int, TestObject>();

            aggregator.Push(1, new TestObject { Id = 1, Name = "Object1" });
            aggregator.Push(1, new TestObject { Id = 1, Name = "Object1_Overwritten" });

            Assert.That(aggregator.Count(), Is.EqualTo(1));
            Assert.That(aggregator.First().Name, Is.EqualTo("Object1_Overwritten"));
        }

        [Test]
        public void Push_ShouldAddNewValues()
        {
            var aggregator = new LatestEntryAggregator<int, TestObject>();

            aggregator.Push(1, new TestObject { Id = 1, Name = "Object1" });
            aggregator.Push(2, new TestObject { Id = 2, Name = "Object2" });

            Assert.That(aggregator.Count(), Is.EqualTo(2));
        }

        [Test]
        public void Clear_ShouldEmptyTheDictionary()
        {
            var aggregator = new LatestEntryAggregator<int, TestObject>();

            aggregator.Push(1, new TestObject { Id = 1, Name = "Object1" });
            aggregator.Push(2, new TestObject { Id = 2, Name = "Object2" });
            aggregator.Clear();

            Assert.That(aggregator.Count(), Is.EqualTo(0));
        }

        [Test]
        public void GetEnumerator_ShouldEnumerateValues()
        {
            var source = new List<TestObject>
            {
                new TestObject { Id = 1, Name = "Object1" },
                new TestObject { Id = 2, Name = "Object2" },
                new TestObject { Id = 3, Name = "Object3" },
            };
            var aggregator = new LatestEntryAggregator<int, TestObject>(source, obj => obj.Id);

            var enumeratedValues = new List<TestObject>();

            foreach (var value in aggregator)
            {
                enumeratedValues.Add(value);
            }

            Assert.That(enumeratedValues.Count, Is.EqualTo(source.Count));
            Assert.IsTrue(source.All(s => enumeratedValues.Any(e => e.Name == s.Name)));
        }
    }
}
