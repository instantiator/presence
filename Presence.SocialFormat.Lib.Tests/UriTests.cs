using Presence.SocialFormat.Lib.Helpers;

namespace Presence.SocialFormat.Tests;

[TestClass]
public class UriTests
{
    [TestMethod]
    [TestCategory("Unit")]
    [DataRow("https://instantiator.dev", true, true, false, true, false)]
    public void Uri_MetadataIsAsExpected(string uriStr, bool expectUri, bool expectLink, bool expectFile, bool expectExists, bool expectImage)
    {
        var ok = Uri.TryCreate(uriStr, UriKind.RelativeOrAbsolute, out var uri);
        Assert.AreEqual(expectUri, ok);
        if (expectUri)
        {
            Assert.IsNotNull(uri);

            var strFromUri = uri.ToString();
            Assert.IsFalse(string.IsNullOrWhiteSpace(strFromUri));
            Assert.AreEqual(new Uri(uriStr), new Uri(strFromUri));

            var metadata = uri.GetMetadata();
            Assert.AreEqual(expectLink, metadata.IsHttp);
            Assert.AreEqual(expectFile, metadata.IsFile);
            Assert.AreEqual(expectExists, metadata.Exists);
            Assert.AreEqual(expectImage, metadata.IsImage);
        }
        else
        {
            Assert.IsNull(uri);
        }
    }

    [TestMethod]
    [TestCategory("Unit")]
    [DataRow("file:/SampleData/icon.png")]
    [DataRow("file:///SampleData/icon.png")]
    public void PathString_ToUri_Succeeds(string str)
    {
        //var uri = // str.ToUri();
        var uriStr = "file://" + Path.GetFullPath(str.Substring("file:".Length).TrimStart('/'));
        Uri.TryCreate(uriStr, UriKind.Absolute, out var result);
        Assert.IsNotNull(result, $"{uriStr}");

        var uri = str.ToUri();
        Assert.IsNotNull(uri, $"{str}");
    }

    [TestMethod]
    [TestCategory("Unit")]
    [DataRow("http://instantiator.dev")]
    [DataRow("http://instantiator.dev/")]
    [DataRow("https://instantiator.dev")]
    [DataRow("https://instantiator.dev/")]
    public void UrlString_ToUri_Succeeds(string str)
    {
        var uri = str.ToUri();
        Assert.IsNotNull(uri, $"{str}");
    }

    [TestMethod]
    [TestCategory("Unit")]
    [DataRow("https://instantiator.dev")]
    [DataRow("file:///SampleData/icon.png")]
    public async Task String_ToUri_CanRead(string str)
    {
        var uri = str.ToUri();
        Assert.IsNotNull(uri);
        var metadata = uri.GetMetadata();
        Assert.IsTrue(metadata.Exists);
        var stream = await uri.GetStreamAsync();
        Assert.IsNotNull(stream);
        stream.Dispose();

    }

}