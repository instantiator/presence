# Create threads with markdown

A heavily simplified markdown-a-like format can be used to represent thread content, which can be interpreted by the `InputReader` - and is supported through the console application.

Draft your content using simple markdown links, images, and text.

Examples are in the `SampleData/` directory.

## Text and post breaks

```markdown
Here is some text that will be used to create posts. It can run on for as long as you like, and it will be wrapped into a thread of posts.

Leaving a gap between text will force a 'break' - ie. it will start a new post in the thread.
```

## Links

You can post links in markdown format, and these will either be displayed as a hyperlink that shows your text, or as a url, depending on the requirements of the network you're posting to.

```markdown
[Presence](https://instantiator.dev/presence)
```

You can also post raw links.

```markdown
https://instantiator.dev/presence
```

## Images

You can post images in markdown format, and these will be displayed in the post that contains the text that they follow.

```markdown
![Presence icon](https://instantiator.dev/presence/images/icon.png)
```

## Headings and other formatting

Other forms of formatting (headings, bold, italics, underline, strikethrough...) are ignored - these are not well supported across social networks.
