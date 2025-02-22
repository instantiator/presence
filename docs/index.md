<div style="float: right; margin-left: 2rem; text-align: center;">
  <img src="images/icon.png" alt="The Presence icon (for now) - an envelope, with thick purple outlines, on a white-to-light-purple gradient" />
</div>

# Presence

[![License badge](https://img.shields.io/github/license/instantiator/presence)](https://github.com/instantiator/presence)
[![Commit activity badge](https://img.shields.io/github/commit-activity/m/instantiator/presence)](https://github.com/instantiator/presence)
[![Last commit badge](https://img.shields.io/github/last-commit/instantiator/presence/main)](https://github.com/instantiator/presence)

<!-- [![Stars badge](https://img.shields.io/github/stars/instantiator/presence)](https://github.com/instantiator/presence)
![Forks badge](https://img.shields.io/github/forks/instantiator/presence) -->

<a class="github-button" href="https://github.com/instantiator/presence" data-color-scheme="no-preference: light; light: light; dark: dark;" data-icon="octicon-star" data-show-count="true" aria-label="Star instantiator/presence on GitHub">Star</a>
<a class="github-button" href="https://github.com/instantiator/presence/fork" data-color-scheme="no-preference: light; light: light; dark: dark;" data-icon="octicon-repo-forked" data-show-count="true" aria-label="Fork instantiator/presence on GitHub">Fork</a>
<a class="github-button" href="https://github.com/instantiator/presence/subscription" data-color-scheme="no-preference: light; light: light; dark: dark;" data-icon="octicon-eye" data-show-count="true" aria-label="Watch instantiator/presence on GitHub">Watch</a>
<a class="github-button" href="https://github.com/instantiator/presence/issues" data-color-scheme="no-preference: light; light: light; dark: dark;" data-icon="octicon-issue-opened" aria-label="Issue instantiator/presence on GitHub">Issue</a>
<a class="github-button" href="https://github.com/instantiator/presence/discussions" data-color-scheme="no-preference: light; light: light; dark: dark;" data-icon="octicon-comment-discussion" aria-label="Discuss instantiator/presence on GitHub">Discuss</a>

Presence is a set of .NET libraries and tools for formatting and sharing threads across social networks.

You can:

- Format threads for multiple social networks, from a single source
- Post threads, messages, and images to multiple social networks

## Just want to use the GitHub Actions?

- [Format and post a thread with GitHub Actions](gha/format-and-post-with-gha.md)
- [How to prepare content](guides/create-with-markdown.md) (in a markdown-like format)
- [Network specific configuration](guides/network-specifics.md)

## Just want to use the CLI tools?

- [How to install the CLI tools](cli/install-cli-tools.md)
- [How to prepare content](guides/create-with-markdown.md) (in a markdown-like format)
- [How to post to social networks](cli/presence-posting-console.md)
- [Network specific configuration](guides/network-specifics.md)

## Network support

| Network        | Notes                    | Thread composition | Thread posting | Image posting |
| -------------- | ------------------------ | ------------------ | -------------- | ------------- |
| `Console`      | Used for testing         | ✅                 | ✅             | ✔️            |
| `AT`           | AT networks, eg. BlueSky | ✅                 | ✅             | ✅            |
| `SlackWebhook` | Simple Slack posting     | ✅                 | ⏳             | ⏳            |

<details>
  <summary><b>⏳ Clarifications...</b></summary>
  <br/>
  <ul>
    <li><b>The <code>Console</code> network does not really post images.</b>
      <ul>
        <li>It indicates the source of the image that would have been posted. This is the intended behaviour.</li>
      </ul>
    </li>
    <li><b><code>SlackWebhook</code> does not yet support posting of threads.</b>
      <ul>
        <li>In practice this is almost irrelevant - Slack permits posts of up to 40,000 characters, which is significantly larger than many other social networks. If a post exceeds this limit, it will be continued in a subsequent top-level post.</li>
        <li><i>The intended behavior, however, is to send subsequent posts as replies to the original message.</i></li>
      </ul>
    </li>
    <li><b><code>SlackWebhook</code> permits posting images by URL, but does not support upload of local images.</b>
      <ul>
        <li>Images that cannot be included in a post to slack will result in a warning.</li>
        <li>A good solution for this will <a href="https://github.com/instantiator/presence/issues/35#issue-2871104974">need a little investigation</a>.</li>
      </ul>
    </li>
  </ul>
</details>

## Status

> **NB. This project is currently under development.** Libraries are not production-ready yet.

| Artefact           | Type            | Location                                                                                                        | Status                                                                                                                                                                                                                           |
| ------------------ | --------------- | --------------------------------------------------------------------------------------------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| Latest source      | GitHub workflow | [Latest build and test](https://github.com/instantiator/presence/actions/workflows/on-push-build-and-test.yaml) | ![Build and test](https://img.shields.io/github/actions/workflow/status/instantiator/presence/on-push-build-and-test.yaml?label=Build%20and%20test)                                                                              |
| CLI binaries       | Homebrew tap    | [presence-cli formula](https://github.com/instantiator/homebrew-presence)                                       | ![Homebrew tap](https://img.shields.io/badge/dynamic/json.svg?url=https://raw.githubusercontent.com/instantiator/homebrew-presence/main/Info/presence-cli.json&query=$.versions.stable&label=instantiator/presence/presence-cli) |
| CLI binaries       | GitHub release  | [Latest release](https://github.com/instantiator/presence/releases/latest)                                      | ![GitHub Release](https://img.shields.io/github/v/release/instantiator/presence?include_prereleases&label=instantiator/presence:latest)                                                                                          |
| Formatting library | Nuget package   | [`Presence.SocialFormat.Lib`](https://www.nuget.org/packages/Presence.SocialFormat.Lib/)                        | ![NuGet Version](https://img.shields.io/nuget/v/Presence.SocialFormat.Lib?label=Presence.SocialFormat.Lib)                                                                                                                       |
| Posting library    | Nuget package   | [`Presence.Posting.Lib`](https://www.nuget.org/packages/Presence.Posting.Lib/)                                  | ![NuGet Version](https://img.shields.io/nuget/v/Presence.Posting.Lib?label=Presence.Posting.Lib)                                                                                                                                 |

## All documentation

### GitHub Actions

- [Format and post a thread with GitHub Actions](gha/format-and-post-with-gha.md)

### CLI tools

- [Install the Presence CLI tools](cli/install-cli-tools.md)
- [Using `Presence.SocialFormat.Console`](cli/presence-social-format-console.md)
- [Using `Presence.Posting.Console`](cli/presence-posting-console.md)

### .NET library and user guides

- [Install packages from Nuget](guides/install-packages.md)
- [Create a thread with `Presence.SocialFormat.Lib`](guides/create-thread.md)
- [Create posts with images](guides/create-images.md)
- [Create threads with markdown](guides/create-with-markdown.md)
- [Post to a social network with `Presence.Posting.Lib`](guides/send-post.md)

### Configuration

- [Network specific constraints](guides/network-constraints.md)
- [Network specific configuration](guides/network-specifics.md)
  - [Set up a `Console` network](guides/networks/setup-console.md)
  - [Set up an `AT` network](guides/networks/setup-at.md)
  - [Set up a `Slack` network](guides/networks/setup-slack.md)

### Developer notes

- [Presence project](https://github.com/users/instantiator/projects/1/views/1)
- [Contributing guidelines](CONTRIBUTING.md)
- [Package versioning](dev-notes/package-versioning.md)
- [Testing](dev-notes/testing.md)

## Source code

All code in this project is distributed under the [MIT](https://github.com/instantiator/presence/blob/main/LICENSE) license. Source code is available at: [instantiator/presence](https://github.com/instantiator/presence)

| Project                         | Purpose                                                                            | Project README                                                                                       |
| ------------------------------- | ---------------------------------------------------------------------------------- | ---------------------------------------------------------------------------------------------------- |
| `Presence.SocialFormat.Lib`     | A library for composing threads, formatted to the requirements of social networks. | [README](https://github.com/instantiator/presence/blob/main/Presence.SocialFormat.Lib/README.md)     |
| `Presence.SocialFormat.Tests`   | Unit tests for the composition and formatting library.                             | [README](https://github.com/instantiator/presence/blob/main/Presence.SocialFormat.Tests/README.md)   |
| `Presence.SocialFormat.Console` | A command line tool to reformat messages for social networks.                      | [README](https://github.com/instantiator/presence/blob/main/Presence.SocialFormat.Console/README.md) |
| `Presence.Posting.Lib`          | A library for posting to social networks.                                          | [README](https://github.com/instantiator/presence/blob/main/Presence.Posting.Lib/README.md)          |
| `Presence.Posting.Tests`        | Unit and integration tests for the posting library.                                | [README](https://github.com/instantiator/presence/blob/main/Presence.Posting.Tests/README.md)        |
| `Presence.Posting.Console`      | A command line tool to post formatted threads to social networks.                  | [README](https://github.com/instantiator/presence/blob/main/Presence.Posting.Console/README.md)      |

_Dependencies for each project are listed in each project's `README`._

## Acknowledgements

- Post / envelope icon from [icons8.com](https://icons8.com)
- Huge thanks to [Gérald Barré](https://bsky.app/profile/meziantou.net) ([meziantou](https://github.com/meziantou)) for this excellent article: [Publishing a Nuget Package](https://www.meziantou.net/publishing-a-nuget-package-following-best-practices-using-github.htm)

<!-- Place this tag in your head or just before your close body tag. -->
<script async defer src="https://buttons.github.io/buttons.js"></script>