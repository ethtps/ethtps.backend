using ETHTPS.Configuration;
using ETHTPS.Configuration.Database;
using ETHTPS.Configuration.Extensions;
using ETHTPS.Data.Core.Models.DataEntries;

namespace ETHTPS.Tests.Data
{
    [TestFixture]
    public class BasicTests
    {
        [Test]
        public void IsIEnumerableWorks()
        {
            Assert.That(typeof(IEnumerable<Block>).IsIEnumerable());
            Assert.That(typeof(IEnumerable<>).IsIEnumerable());
            Assert.That(typeof(IEnumerable<int>).IsIEnumerable());
            Assert.That(typeof(IEnumerable<ConfigurationString>).IsIEnumerable());
            Assert.That(typeof(IEnumerable<IConfigurationString>).IsIEnumerable());
            Assert.That(typeof(IEnumerable<IConfigurationString?>).IsIEnumerable());
            Assert.That(typeof(IEnumerable<IConfigurationString>).IsIEnumerable());
        }
    }
}
