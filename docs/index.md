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

| Source code                                                                                            | Status                                                                                                                                              |
| ------------------------------------------------------------------------------------------------------ | --------------------------------------------------------------------------------------------------------------------------------------------------- |
| [Latest build](https://github.com/instantiator/presence/actions/workflows/on-push-build-and-test.yaml) | ![Build and test](https://img.shields.io/github/actions/workflow/status/instantiator/presence/on-push-build-and-test.yaml?label=Build%20and%20test) |

| CLI binaries                                                               | Status                                                                                                                                                                                                                             |
| -------------------------------------------------------------------------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| [Homebrew tap](https://github.com/instantiator/homebrew-presence)          | ![Homebrew tap](https://img.shields.io/badge/dynamic/json.svg?url=https://raw.githubusercontent.com/instantiator/homebrew-presence/master/Info/presence-cli.json&query=$.versions.stable&label=instantiator/presence/presence-cli) |
| [Latest release](https://github.com/instantiator/presence/releases/latest) | ![GitHub Release](https://img.shields.io/github/v/release/instantiator/presence?include_prereleases&label=instantiator/presence:latest)                                                                                            |

| Nuget packages                                                                           | Status                                                                                                     |
| ---------------------------------------------------------------------------------------- | ---------------------------------------------------------------------------------------------------------- |
| [`Presence.SocialFormat.Lib`](https://www.nuget.org/packages/Presence.SocialFormat.Lib/) | ![NuGet Version](https://img.shields.io/nuget/v/Presence.SocialFormat.Lib?label=Presence.SocialFormat.Lib) |
| [`Presence.Posting.Lib`](https://www.nuget.org/packages/Presence.Posting.Lib/)           | ![NuGet Version](https://img.shields.io/nuget/v/Presence.Posting.Lib?label=Presence.Posting.Lib)           |

## Network support

| Network   | Notes            | Thread composition | Thread posting | Image posting |
| --------- | ---------------- | ------------------ | -------------- | ------------- |
| `AT`      | ie. BlueSky      | ✅                 | ✅             | ✅            |
| `Console` | Used for testing | ✅                 | ✅             | ✅            |

## CLI tools

- [Install the Presence CLI tools](cli/install-cli-tools.md)
- [Using `Presence.SocialFormat.Console`](cli/presence-social-format-console.md)
- [Using `Presence.Posting.Console`](cli/presence-posting-console.md)

## .NET library user guide

- [Install packages from Nuget](guides/install-packages.md)
- [Create a thread with `Presence.SocialFormat.Lib`](guides/create-thread.md)
- [Create posts with images](guides/create-images.md)
- [Create threads with markdown](guides/create-with-markdown.md)
- [Post to a social network with `Presence.Posting.Lib`](guides/send-post.md)
- [Network specific configuration](guides/network-specifics.md)

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
