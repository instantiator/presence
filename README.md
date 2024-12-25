# Presence

Presence is a set of .NET libraries and tools for social sharing.

All code here is distributed under the [MIT](https://github.com/instantiator/presence/blob/main/LICENSE) license.

## Prerequisites

- [.NET 8.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) (download link)

## Projects

| Project                | Purpose                                                                                      |
| ---------------------- | -------------------------------------------------------------------------------------------- |
| `SocialFormat.Console` | A command line tool to reformat messages for social networks.                                |
| `SocialFormat.Lib`     | A library for composing posts and threads, formatted to the requirements of social networks. |
| `Presence.Lib`         | A library for posting cross-network format posts to social networks.                         |
| `SocialFormat.Tests`   | Unit tests for the composition and formatting library.                                       |

## `SocialFormat.Console`

This is a command line tool to reformat text, images, links, and hashtags into a usable thread for one or more social networks. It will:

- Number each post in the thread, if specified.
- Add specified hashtags to the first post, or all posts, as specified.
- Ensure all posts fit within the maximum length specified for the target social network.

Usage:

- Run without parameters for help information: `./format.sh`
- The input is a path to a JSON file containing a [`CompositionRequest`](https://github.com/instantiator/presence/blob/main/SocialFormat.Lib/DTO/CompositionRequest.cs).

## `SocialFormat.Lib`

- Provides the `CommonPost` and `SocialSnippet` classes - used to assemble messages that can be formatted for distribution across any social network.
- Provides thread composition classes, and the `IThreadComposer` interface for interacting with them. Threads are formatted into `CommonPosts` from `SocialSnippets`.

## `Presence.Lib`

## Testing

Run all tests in the solution with `run-tests.sh`

## Dependencies

### `SocialFormat.Console`

| Dependency                                                            | Creator                                                   | License                                                                        | Usage                                                           |
| --------------------------------------------------------------------- | --------------------------------------------------------- | ------------------------------------------------------------------------------ | --------------------------------------------------------------- |
| [CommandLineParser](https://github.com/commandlineparser/commandline) | [commandlineparser](https://github.com/commandlineparser) | [MIT](https://github.com/commandlineparser/commandline/blob/master/License.md) | Interpret command line parameters.                              |
| [SocialFormat.Lib](https://github.com/instantiator/presence)          | [Lewis Westbury](https://github.com/instantiator)         | [MIT](https://github.com/instantiator/presence/blob/main/LICENSE)              | Common post format, preparing and formatting posts and threads. |

### `Presence.Lib`

| Dependency                                                   | Creator                                           | License                                                           | Usage                                                           |
| ------------------------------------------------------------ | ------------------------------------------------- | ----------------------------------------------------------------- | --------------------------------------------------------------- |
| [SocialFormat.Lib](https://github.com/instantiator/presence) | [Lewis Westbury](https://github.com/instantiator) | [MIT](https://github.com/instantiator/presence/blob/main/LICENSE) | Common post format, preparing and formatting posts and threads. |
