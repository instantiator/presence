# Installing the Presence CLI tools

The CLI tools are available as binaries attached to each release and available through Homebrew on Mac OS and Linux. Either:

1. Tap and install `presence-cli` through Homebrew, or
2. Download the binaries directly, and put them in your path

## Option 1: Install with Homebrew

- If you haven't already, install [Homebrew](https://brew.sh/)

- Tap `instantiator/presence`

  ```bash
  brew tap instantiator/presence
  ```

- Install the `presence-cli` formula

  ```bash
  brew install instantiator/presence/presence-cli
  ```

- Test each binary has installed successfully

  ```bash
  Presence.SocialFormat.Console --version
  Presence.Posting.Console --version
  ```

The homebrew formula will install standalone binaries that do not have a dependency on the .NET 8 runtime.

## Option 2: Download binaries directly

Download and extract the appropriate binary for your system.

- Visit: [latest release](https://github.com/instantiator/presence/releases/latest)
- Download the `Presence.zip` - containing all binaries
- Unzip and locate your binaries
- Place those binaries in your path, or in the directory you wish to use them from

### Choosing a binary

Pick a binary for your operating system.

| System  | Directory   | Binary                              | Stand-alone binary                                 |
| ------- | ----------- | ----------------------------------- | -------------------------------------------------- |
| Linux   | `linux-x64` | `Presence.SocialFormat.Console`     | `self-contained/Presence.SocialFormat.Console`     |
| Linux   | `linux-x64` | `Presence.Posting.Console`          | `self-contained/Presence.Posting.Console`          |
| Mac OS  | `osx-x64`   | `Presence.SocialFormat.Console`     | `self-contained/Presence.SocialFormat.Console`     |
| Mac OS  | `osx-x64`   | `Presence.Posting.Console`          | `self-contained/Presence.Posting.Console`          |
| Windows | `win-x64`   | `Presence.SocialFormat.Console.exe` | `self-contained/Presence.SocialFormat.Console.exe` |
| Windows | `win-x64`   | `Presence.Posting.Console.exe`      | `self-contained/Presence.Posting.Console.exe`      |

NB. Each binary comes in 2 versions:

- **Dependent:** Smaller binary, requires an installation of the .NET 8 runtime
- **Self-contained:** Larger, has no dependency on .NET 8

### Debugging information

If you encounter any unexpected behaviour while using the tools, please report this by [creating an issue](https://github.com/instantiator/presence/issues/new).

The `.pdb` and `.xml` files present contain debugging information which will generate helpful stack traces should the application fail when using it. Provided these are found in the same directory as the binary they're related to, you should be able to generate some additional, helpful information to add to your report.
