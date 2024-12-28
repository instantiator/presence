# Presence

Presence is a set of .NET libraries and tools for social sharing.

> [!WARNING]
> NB. This project is currently under development - libraries are not production ready yet.

All code here is distributed under the [MIT](https://github.com/instantiator/presence/blob/main/LICENSE) license.

Source code is at: [instantiator/presence](https://github.com/instantiator/presence)

## Prerequisites

- [.NET 8.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) (download link)

## User guide

- [Install packages from Nuget](guides/install-packages.md)
- [Create a thread with Presence.SocialFormat.Lib](guides/create-thread.md)

## Developer notes

- [Presence](https://github.com/users/instantiator/projects/1/views/1) (GitHub project)
- [Package versioning](dev-notes/package-versioning.md)

## Projects

| Project                         | Purpose                                                                            | Project README                                                                    |
| ------------------------------- | ---------------------------------------------------------------------------------- | --------------------------------------------------------------------------------- |
| `Presence.SocialFormat.Lib`     | A library for composing threads, formatted to the requirements of social networks. | [README](https://github.com/instantiator/presence/Presence.SocialFormat.Lib/README.md)     |
| `Presence.SocialFormat.Console` | A command line tool to reformat messages for social networks.                      | [README](https://github.com/instantiator/presence/Presence.SocialFormat.Console/README.md) |
| `Presence.Posting.Lib`          | A library for posting cross-network format posts to social networks.               | [README](https://github.com/instantiator/presence/Presence.Posting.Lib/README.md)         |
| `Presence.SocialFormat.Tests`   | Unit tests for the composition and formatting library.                             | [README](https://github.com/instantiator/presence/Presence.SocialFormat.Tests/README.md)   |

_Dependencies for each project are listed in each project's `README`._

## Acknowledgements

- Post icon from [icons8.com](https://icons8.com)
- Huge thanks to [Gérald Barré](https://bsky.app/profile/meziantou.net) ([meziantou](https://github.com/meziantou)) for this excellent article: [Publishing a Nuget Package](https://www.meziantou.net/publishing-a-nuget-package-following-best-practices-using-github.htm)
