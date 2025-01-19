# Create a thread

`Presence.SocialFormat.Lib` provides a `ThreadBuilder` class, which can be used to build threads, based on a set of `SocialSnippet` inputs, for any number of different networks.

Each network has a number of rules, described with structs `ThreadCompositionRules`, `PostRenderRules`, and an optional `ICounterCreator` (the default counter is fairly sensible).

Alternatively, you can reach the thread composers directly. Each implements the `IThreadComposer` interface, allowing you to compose a thread from a collection of `Snippets`.

## Snippets

A `SocialSnippet` is a piece of a post. `SnippetType` is an `enum` with several possible values, presenting the type of snippet:

- `Text` - simple text (place the text in `.Text`)
- `Link` - an external link (place link text in `.Text`, and the link url in `.Reference`)
- `Tag` - a HashTag (place the tag in the `.Text` value)
- `Break` - a special snippet to indicate that the thread composer must start a new post
- `Counter` - a special text snippet created by the thread composer as a post number indicator
- `Image` - this is _one way_ to represent an image (see: [Images in threads](create-images.md))

Initialse a `SocialSnippet` with a simple constructor or the initialisation syntax:

```csharp
var snippet1 = new SocialSnippet("Simple text");
var snippet2 = new SocialSnippet("This is a link", SnippetType.Link, "https://instantiator.dev");
var snippet3 = new SocialSnippet()
{
    Text = "#Tagging",
    SnippetType = SocialSnippetType.Tag
};
```

## Building a thread

Here's a the simplest possible example - add a single Message snippet (of `SnippetType.Text` by default), and build the thread.

```csharp
var posts = new ThreadBuilder(SocialNetwork.AT)
    .WithMessage(new SocialSnippet("Hello, world!"))
    .Build();
```

The `ThreadBuilder.Build()` method returns an `IDictionary<IThreadComposer, IEnumerable<CommonPost>>` - a dictionary keyed by composers, with values of the thread that each composer created.

You could add any number of additional snippets, together as an `IEnumerable<SocialSnippet>` or individually.

Set tags for the thread, too - similarly, individually or together as an `IEnumerable<SocialSnippet>`:

```csharp
var posts = new ThreadBuilder(SocialNetwork.AT)
    .WithMessages(messageSnippets)
    .WithTag(new SocialSnippet("#Example1", SnippetType.Tag))
    .WithTag(new SocialSnippet("#Example2", SnippetType.Tag))
    .Build();
```

By default, the `ATThreadComposer` will

- limit text to `300` characters per post (including counters, hashtags)
- render links as text rather than URLs (as AT/BlueSky can specify link regions in posts)
- apply hashtags to the first post only
- apply a counter to the beginning of each post, if there is more than 1 post in the thread
