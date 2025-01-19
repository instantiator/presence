<img src="images/icon.png" style="float: right;" />

# Presence

Presence is a set of .NET libraries and tools for social sharing.

You can:

- Format threads for multiple social networks, from a single source
- Post threads, messages, and images (coming soon!) to social networks

All code here is distributed under the [MIT](https://github.com/instantiator/presence/blob/main/LICENSE) license.

Source code is at: [instantiator/presence](https://github.com/instantiator/presence)

## Status

> **Warning.** This project is currently under development - libraries are not production-ready yet.

[![on-push-build-and-test](https://github.com/instantiator/presence/actions/workflows/on-push-build-and-test.yaml/badge.svg?branch=main)](https://github.com/instantiator/presence/actions/workflows/on-push-build-and-test.yaml)

| Package                                                                                  | NuGet version                                                              |
| ---------------------------------------------------------------------------------------- | -------------------------------------------------------------------------- |
| [`Presence.SocialFormat.Lib`](https://www.nuget.org/packages/Presence.SocialFormat.Lib/) | ![NuGet Version](https://img.shields.io/nuget/v/Presence.SocialFormat.Lib) |
| [`Presence.Posting.Lib`](https://www.nuget.org/packages/Presence.Posting.Lib/)           | ![NuGet Version](https://img.shields.io/nuget/v/Presence.Posting.Lib)      |

## Network support

| Network      | Thread composition | Thread posting | Image posting |
| ------------ | ------------------ | -------------- | ------------- |
| AT (BlueSky) | ✅                 | ✅             |               |

## Prerequisites

- [.NET 8.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) (download link)

## User guide

- [Install packages from Nuget](guides/install-packages.md)
- [Create a thread with `Presence.SocialFormat.Lib`](guides/create-thread.md)
- [Create posts with images](guides/create-images.md)
- [Create threads with markdown](guides/create-with-markdown.md)
- [Post to a social network with `Presence.Posting.Lib`](guides/send-post.md)
- [Network specific configuration](guides/network-specifics.md)

## CLI tools

- [Downloading binaries](cli/download-binaries.md)
- [Using `Presence.SocialFormat.Console`](cli/presence-social-format-console.md)

## Developer notes

- [Presence project](https://github.com/users/instantiator/projects/1/views/1)
- [Contributing guidelines](CONTRIBUTING.md)
- [Package versioning](dev-notes/package-versioning.md)
- [Testing](dev-notes/testing.md)

## Projects

| Project                         | Purpose                                                                            | Project README                                                                                       |
| ------------------------------- | ---------------------------------------------------------------------------------- | ---------------------------------------------------------------------------------------------------- |
| `Presence.SocialFormat.Lib`     | A library for composing threads, formatted to the requirements of social networks. | [README](https://github.com/instantiator/presence/blob/main/Presence.SocialFormat.Lib/README.md)     |
| `Presence.SocialFormat.Tests`   | Unit tests for the composition and formatting library.                             | [README](https://github.com/instantiator/presence/blob/main/Presence.SocialFormat.Tests/README.md)   |
| `Presence.SocialFormat.Console` | A command line tool to reformat messages for social networks.                      | [README](https://github.com/instantiator/presence/blob/main/Presence.SocialFormat.Console/README.md) |
| `Presence.Posting.Lib`          | A library for posting to social networks.                                          | [README](https://github.com/instantiator/presence/blob/main/Presence.Posting.Lib/README.md)          |
| `Presence.Posting.Tests`        | Unit and integration tests for the posting library.                                | [README](https://github.com/instantiator/presence/blob/main/Presence.Posting.Tests/README.md)        |

_Dependencies for each project are listed in each project's `README`._

## Acknowledgements

- Post icon from [icons8.com](https://icons8.com)
- Huge thanks to [Gérald Barré](https://bsky.app/profile/meziantou.net) ([meziantou](https://github.com/meziantou)) for this excellent article: [Publishing a Nuget Package](https://www.meziantou.net/publishing-a-nuget-package-following-best-practices-using-github.htm)
