using System.Text.Json;
using Presence.SocialFormat.Lib.IO.Text;
using Presence.SocialFormat.Lib.Posts;

namespace Presence.SocialFormat.Tests;

[TestClass]
[TestCategory("Unit")]
public class MarkdownFormatParserTests
{
    [TestMethod]
    public void MarkdownFormatParser_CanReadCombinedContent()
    {
        var content = @"Some text goes here.
[This is a link](https://instantiator.dev) ![This is an image](https://instantiator.dev/lewis/profile.jpg)

Some more text - after a break. #TagOne #Tag2

Another break, and now another link [ICGames](https://icgames.net/)";

        var parser = new MarkdownFormatParser();
        var request = parser.ToRequest(content);

        Assert.AreEqual(22, request.Message.Count());
        Assert.AreEqual(2, request.Message.Count(s => s.SnippetType == SnippetType.Break), JsonSerializer.Serialize(request, new JsonSerializerOptions { WriteIndented = true }));
        Assert.AreEqual(2, request.Message.Count(s => s.SnippetType == SnippetType.Link), JsonSerializer.Serialize(request, new JsonSerializerOptions { WriteIndented = true }));
        Assert.AreEqual(1, request.Message.Count(s => s.SnippetType == SnippetType.Image), JsonSerializer.Serialize(request, new JsonSerializerOptions { WriteIndented = true }));
        Assert.AreEqual(2, request.Tags.Count(), JsonSerializer.Serialize(request, new JsonSerializerOptions { WriteIndented = true }));

        Assert.AreEqual("This is a link", request.Message.First(s => s.SnippetType == SnippetType.Link).Text);
        Assert.AreEqual("https://instantiator.dev", request.Message.First(s => s.SnippetType == SnippetType.Link).Reference);

        Assert.AreEqual("This is an image", request.Message.First(s => s.SnippetType == SnippetType.Image).Text);
        Assert.AreEqual("https://instantiator.dev/lewis/profile.jpg", request.Message.First(s => s.SnippetType == SnippetType.Image).Reference);

        Assert.AreEqual("ICGames", request.Message.Last(s => s.SnippetType == SnippetType.Link).Text);
        Assert.AreEqual("https://icgames.net/", request.Message.Last(s => s.SnippetType == SnippetType.Link).Reference);

        Assert.AreEqual("TagOne", request.Tags.First().Text);
        Assert.AreEqual("Tag2", request.Tags.Last().Text);
    }

}
