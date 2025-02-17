using Presence.SocialFormat.Lib.Networks;

namespace Presence.SocialFormat.Lib.Tests;

[TestClass]
[TestCategory("Unit")]
public class FactoryTests
{
    [TestMethod]
    public void ComposerFactory_CanCreate_AllThreadComposers()
    {
        foreach (var network in Enum.GetValues<SocialNetwork>())
        {
            var composer = ComposerFactory.ForNetwork(network);
            Assert.IsNotNull(composer, $"ComposerFactory could not create an IThreadComposer for: {network}");
            Assert.AreEqual(network, composer.Identity.Network);
        }
    }
}
