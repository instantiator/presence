# Presence.SocialFormat.Console

> [!WARNING]
> NB. This project is currently under development - libraries are not production ready yet.

This is a command line tool to reformat text, images, links, and hashtags into a usable thread for one or more social networks. It will:

- Number each post in the thread, if specified.
- Add specified hashtags to the first post, or all posts, as specified.
- Ensure all posts fit within the maximum length specified for the target social network.

## Getting started

See:

- [Presence documentation](https://instantiator.dev/presence)
- [Using `Presence.SocialFormat.Console`](https://instantiator.dev/presence/cli/presence-social-format-console)
- [Using `Presence.Posting.Console`](https://instantiator.dev/presence/cli/presence-posting-console)

## Dependencies

### `Presence.SocialFormat.Console`

| Dependency                                                            | Creator                                                   | License                                                                           | Usage                                                           |
| --------------------------------------------------------------------- | --------------------------------------------------------- | --------------------------------------------------------------------------------- | --------------------------------------------------------------- |
| [Presence.SocialFormat.Lib](https://github.com/instantiator/presence) | [Lewis Westbury](https://github.com/instantiator)         | [MIT](https://github.com/instantiator/presence/blob/main/LICENSE)                 | Common post format, preparing and formatting posts and threads. |
| [CommandLineParser](https://github.com/commandlineparser/commandline) | [commandlineparser](https://github.com/commandlineparser) | [MIT](https://github.com/commandlineparser/commandline/blob/master/License.md)    | Interpret command line parameters.                              |
| [MinVer](https://github.com/adamralph/minver)                         | [Adam Ralph](https://github.com/adamralph)                | [Apache 2.0](https://github.com/adamralph/minver?tab=Apache-2.0-1-ov-file#readme) | Manage package versions                                         |
