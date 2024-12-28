# Package versioning

To version a package, first push an appropriate tag, eg.

```bash
git tag social-format-lib-0.1.0
git push --tags
```

> [!NOTE]
> Each published project relies on versioning tags prefixed with a unique identifier, as shown here.

| Package                         | Tag prefix               | Nuget location                                                                         |
| ------------------------------- | ------------------------ | -------------------------------------------------------------------------------------- |
| `Presence.SocialFormat.Lib`     | `social-format-lib-`     | [Presence.SocialFormat.Lib](https://www.nuget.org/packages/Presence.SocialFormat.Lib/) |
| `Presence.SocialFormat.Console` | `social-format-console-` |                                                                                        |

## Preparing binaries

Test the binary build process with provided scripts:

| Script                      | Purpose                                                                       | Output directory |
| --------------------------- | ----------------------------------------------------------------------------- | ---------------- |
| `prepare-cli-tools.sh`      | Build binaries for the CLI tools. Currently: `Presence.SocialFormat.Console`  | `release`        |
| `prepare-nuget-packages.sh` | Build binaries for the Nuget packages. Currently: `Presence.SocialFormat.Lib` | `nuget`          |

## Publication to Nuget

Library packages are published to Nuget.

Create a [release](https://github.com/instantiator/presence/releases) from the main branch, specifying the latest tag you have applied, to trigger build and publication of packages to Nuget.
