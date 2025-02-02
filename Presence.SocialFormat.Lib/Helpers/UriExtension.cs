using MimeTypes;
using Presence.SocialFormat.Lib.Constants;

namespace Presence.SocialFormat.Lib.Helpers;

public struct UriMetadata
{
    public required Uri Uri { get; init; }
    public required bool IsImage { get; init; }
    public required bool IsHttp { get; init; }
    public required bool IsFile { get; init; }
    public required bool Exists { get; init; }
    public required string? MimeType { get; init; }
}

public static class UriExtension
{
    public static Uri? ToUri(this string? str)
    {
        if (string.IsNullOrWhiteSpace(str)) { return null; }
        if (str.ToLower().StartsWith("file:"))
        {
            // detect and adjust for relative paths
            var path = str.Substring("file:".Length).TrimStart('/');
            if (File.Exists(path))
            {
                var absolute = Path.GetFullPath(path);
                str = $"file://{absolute}";
            }
        }
        Uri.TryCreate(str, UriKind.Absolute, out var result);
        return result;
    }

    public static UriMetadata GetMetadata(this Uri uri)
    {
        var isImage = false;
        var isHttp = uri.UriIsHttp();
        var isFile = uri.UriIsFile();
        var exists = false;
        string? mimeType = null;

        if (UriConstants.IMAGE_EXTENSIONS.Any(uri.AbsolutePath.EndsWith)) { isImage = true; }
        if (isHttp)
        {
            // get headers
            using var http = new HttpClient();
            var response = http.SendAsync(new HttpRequestMessage(HttpMethod.Head, uri)).Result;
            if (response.IsSuccessStatusCode)
            {
                exists = true;
                mimeType = response.Content.Headers.ContentType?.MediaType
                    ?? MimeTypeMap.GetMimeType(Path.GetExtension(uri.AbsolutePath));
                if (mimeType?.StartsWith("image/") == true)
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
            mimeType = MimeTypeMap.GetMimeType(Path.GetExtension(uri.LocalPath));
        }

        return new UriMetadata
        {
            Uri = uri,
            IsImage = isImage,
            IsHttp = isHttp,
            IsFile = isFile,
            Exists = exists,
            MimeType = mimeType
        };
    }

    public static bool UriIsHttp(this Uri uri) => uri.Scheme == "http" || uri.Scheme == "https";
    public static bool UriIsFile(this Uri uri) => uri.IsFile || uri.Scheme == "file";

    public static async Task<Stream> GetStreamAsync(this Uri uri, UriMetadata? metadata = null, HttpClient? httpClient = null)
    {
        httpClient ??= new HttpClient();
        metadata ??= uri.GetMetadata();
        return metadata!.Value.IsFile ? File.OpenRead(uri.LocalPath) : await httpClient!.GetStreamAsync(uri);
    }
}