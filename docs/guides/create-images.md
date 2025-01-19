# Create posts with images

## Images as a `SocialSnippet`

As mentioned in [Create a thread](create-thread.md), `SnippetType.Image` can be used to represent an image.

```csharp
var snippet = new SocialSnippet()
{
    Text = "The presence icon (this is the alt text)",
    Reference = "https://instantiator.dev/presence/images/icon.png",
    SnippetType = SnippetType.Image
};
```

Where an `Image` type snippet appears, it will be added to the post it appears in as an image.

## Images on any `SocialSnippet`

Alternatively, you can provide images as a list to any given snippet, eg.

```csharp
var snippet = new SocialSnippet()
{
    Text = "This is a snippet with text",
    SnippetType = SnippetType.Text,
    Images =
    [
        new SocialSnippetImage()
        {
            Src = "https://instantiator.dev/presence/images/icon.png",
            Alt = "The presence icon (this is the alt text)"
        }
    ]
};
```

NB. Images may be rearranged during Thread Composition to meet the maximum number of images per thread requirement of each network.
