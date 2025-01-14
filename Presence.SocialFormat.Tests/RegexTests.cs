using System.Text.RegularExpressions;
using Presence.SocialFormat.Lib.Constants;

namespace Presence.SocialFormat.Tests;

[TestClass]
[TestCategory("Unit")]
public class RegexTests
{
    [TestMethod]
    [DataRow("[instantiator.dev](https://instantiator.dev)", true, "instantiator.dev", "https://instantiator.dev")]
    [DataRow("[This is a link](https://instantiator.dev)", true, "This is a link", "https://instantiator.dev")]
    [DataRow("![instantiator.dev](https://instantiator.dev/lewis/profile.jpg)", true, "instantiator.dev", "https://instantiator.dev/lewis/profile.jpg")]
    public void MD_LINK_REGEX_CanMatchUrls(string input, bool shouldMatch, string? expectedText, string? expectedUri)
    {
        var LinkRegex = new Regex($"^{RegexConstants.MD_LINK_REGEX}");
        var match = LinkRegex.Match(input);
        Assert.AreEqual(shouldMatch, match.Success);

        var text = match.Groups["text"].Value;
        Assert.AreNotEqual(shouldMatch, string.IsNullOrWhiteSpace(text));
        if (shouldMatch) { Assert.AreEqual(expectedText, text); }

        var uriStr = match.Groups["uri"].Value;
        Assert.AreNotEqual(shouldMatch, string.IsNullOrWhiteSpace(uriStr));
        if (shouldMatch) { Assert.AreEqual(expectedUri, uriStr); }
    }

}