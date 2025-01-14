using Presence.SocialFormat.Lib.Constants;

namespace Presence.SocialFormat.Lib.Helpers;

public struct UriMetadata
{
    public Uri Uri { get; init; }
    public bool IsImage { get; init; }
    public bool IsLink { get; init; }
    public bool IsFile { get; init; }
    public bool Exists { get; init; }
}

public static class UriExtension
{
    public static Uri? ToUri(this string? str)
    {
        if (string.IsNullOrWhiteSpace(str)) { return null; }
        var ok = Uri.TryCreate(str, UriKind.Absolute, out var result) ? result : null;
        return result;
    }

    public static UriMetadata GetMetadata(this Uri uri)
    {
        var isImage = false;
        var isLink = uri.UriIsLink();
        var isFile = uri.UriIsFile();
        var exists = false;

        if (UriConstants.IMAGE_EXTENSIONS.Any(uri.AbsolutePath.EndsWith)) { isImage = true; }
        if (isLink)
        {
            // get headers
            using var http = new HttpClient();
            var response = http.SendAsync(new HttpRequestMessage(HttpMethod.Head, uri)).Result;
            if (response.IsSuccessStatusCode)
            {
                exists = true;
                var contentType = response.Content.Headers.ContentType?.MediaType;
                if (contentType?.StartsWith("image/") == true)
                {
                    isImage = true;
                }
            }
            else
            {
                exists = false;
            }
        }
        else if (isFile)
        {
            exists = File.Exists(uri.LocalPath);
        }

        return new UriMetadata
        {
            Uri = uri,
            IsImage = isImage,
            IsLink = isLink,
            IsFile = isFile,
            Exists = exists
        };
    }

    public static bool UriIsLink(this Uri uri) => uri.Scheme == "http" || uri.Scheme == "https";
    public static bool UriIsFile(this Uri uri) => uri.IsFile || uri.Scheme == "file";
}