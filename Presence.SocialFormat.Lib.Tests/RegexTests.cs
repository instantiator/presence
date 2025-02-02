using System.Text.RegularExpressions;
using Presence.SocialFormat.Lib.Constants;

namespace Presence.SocialFormat.Lib.Tests;

[TestClass]
[TestCategory("Unit")]
public class RegexTests
{
    [TestMethod]
    [DataRow("[instantiator.dev](https://instantiator.dev)", true, false, "instantiator.dev", "https://instantiator.dev")]
    [DataRow("[This is a link](https://instantiator.dev)", true, false, "This is a link", "https://instantiator.dev")]
    [DataRow("![instantiator.dev](https://instantiator.dev/lewis/profile.jpg)", false, true, "instantiator.dev", "https://instantiator.dev/lewis/profile.jpg")]
    public void MD_REGEX_CanMatchUrls(string input, bool isLink, bool isImage, string expectedText, string expectedUri)
    {
        var LinkRegex = new Regex($"^{RegexConstants.MD_LINK_REGEX}");
        var linkMatch = LinkRegex.Match(input);
        Assert.AreEqual(isLink, linkMatch.Success);

        var linkText = linkMatch.Groups["text"].Value;
        Assert.AreNotEqual(isLink, string.IsNullOrWhiteSpace(linkText));
        if (isLink) { Assert.AreEqual(expectedText, linkText); }

        var linkUri = linkMatch.Groups["uri"].Value;
        Assert.AreNotEqual(isLink, string.IsNullOrWhiteSpace(linkUri));
        if (isLink) { Assert.AreEqual(expectedUri, linkUri); }

        var ImageRegex = new Regex($"^{RegexConstants.MD_IMAGE_REGEX}");
        var imageMatch = ImageRegex.Match(input);
        Assert.AreEqual(isImage, imageMatch.Success);

        var imageText = imageMatch.Groups["text"].Value;
        Assert.AreNotEqual(isImage, string.IsNullOrWhiteSpace(imageText));
        if (isImage) { Assert.AreEqual(expectedText, imageText); }

        var imageUri = imageMatch.Groups["uri"].Value;
        Assert.AreNotEqual(isImage, string.IsNullOrWhiteSpace(imageUri));
        if (isImage) { Assert.AreEqual(expectedUri, imageUri); }

    }
}