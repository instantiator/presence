# Presence

Presence is a set of .NET libraries and tools for social sharing.

All code here is distributed under the [MIT](https://github.com/instantiator/presence/blob/main/LICENSE) license.

## Prerequisites

* .NET 8.0

## Libraries

| Library            | Purpose                                                                                      |
| ------------------ | -------------------------------------------------------------------------------------------- |
| `SocialFormat.Lib` | A library for composing posts and threads, formatted to the requirements of social networks. |
| `Presence.Lib`     | A library for posting cross-network format posts to social networks.                         |

## `SocialFormat.Lib`

- Provides the `CommonPost` and `SocialSnippet` classes - used to assemble messages that can be formatted for distribution across any social network.
- Provides thread composition classes, and the `IThreadComposer` interface for interacting with them. Threads are formatted into `CommonPosts` from `SocialSnippets`.

## `Presence.Lib`

`Presence.Lib` relies on a number of supporting libraries:

| Dependency                                                   | Creator                                           | License                                                           | Usage                                                           |
| ------------------------------------------------------------ | ------------------------------------------------- | ----------------------------------------------------------------- | --------------------------------------------------------------- |
| [SocialFormat.Lib](https://github.com/instantiator/presence) | [Lewis Westbury](https://github.com/instantiator) | [MIT](https://github.com/instantiator/presence/blob/main/LICENSE) | Common post format, preparing and formatting posts and threads. |

## Testing

Run all tests in the solution with `run-tests.sh`
