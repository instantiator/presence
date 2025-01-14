using Presence.SocialFormat.Lib.Helpers;

namespace Presence.SocialFormat.Tests;

[TestClass]
[TestCategory("Unit")]
public class UriTests
{
    [TestMethod]
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
            Assert.AreEqual(expectLink, metadata.IsLink);
            Assert.AreEqual(expectFile, metadata.IsFile);
            Assert.AreEqual(expectExists, metadata.Exists);
            Assert.AreEqual(expectImage, metadata.IsImage);
        }
        else
        {
            Assert.IsNull(uri);
        }

    }

}