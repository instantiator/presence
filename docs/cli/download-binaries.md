# Downloading binaries

The CLI tools are available as binaries attached to each release.

## Finding the right binary

Download and extract the appropriate binary for your system.

- Visit: [latest release](https://github.com/instantiator/presence/releases/latest)
- Download the `Presence.zip` - containing all binaries
- Unzip and locate your binary

### Choosing a binary

Pick a binary for your operating system.

| System  | Directory   | Binary                              | Stand-alone binary                                 |
| ------- | ----------- | ----------------------------------- | -------------------------------------------------- |
| Linux   | `linux-x64` | `Presence.SocialFormat.Console`     | `self-contained/Presence.SocialFormat.Console`     |
| Mac OS  | `osx-x64`   | `Presence.SocialFormat.Console`     | `self-contained/Presence.SocialFormat.Console`     |
| Windows | `win-x64`   | `Presence.SocialFormat.Console.exe` | `self-contained/Presence.SocialFormat.Console.exe` |

NB. Each binary comes in 2 versions:

- **Dependent:** Smaller binary, needs an installation of .NET 8 on your system
- **Self-contained:** Larger, can run without dependencies

### Debugging information

If you encounter any unexpected behaviour while using the tools, please report this by [creating an issue](https://github.com/instantiator/presence/issues/new).

The `.pdb` and `.xml` files present contain debugging information which will generate helpful stack traces should the application fail when using it. Provided these are found in the same directory as the binary they're related to, you should be able to generate some additional, helpful information to add to your report.
