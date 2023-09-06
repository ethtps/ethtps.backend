using ETHTPS.Configuration.AutoSetup.Scripts;

namespace ETHTPS.Tests.AutoSetup;

[TestFixture]
public sealed class AutoSetupTests
{
    private readonly ETHTPSAutoSetupScript _mainScript = new("Test");

    [SetUp]
    public void Setup()
    {

    }

    [Test]
    public void BasicRunTest()
    {
        Assert.DoesNotThrow(() =>
        {
            _mainScript.Run();
        });
    }
}