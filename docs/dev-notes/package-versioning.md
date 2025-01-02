# Package versioning

To version a package, first attach an appropriate tag to the current commit, and push with tags, eg.

```bash
git tag 0.2.1
git push --tags
```

> [!NOTE]
> For now, all packages share a common version.

## Preparing binaries

Test the binary build process with provided scripts:

| Script                      | Purpose                                                                       | Output directory |
| --------------------------- | ----------------------------------------------------------------------------- | ---------------- |
| `prepare-cli-binaries.sh`   | Build binaries for the CLI tools. Currently: `Presence.SocialFormat.Console`  | `release`        |
| `prepare-nuget-packages.sh` | Build binaries for the Nuget packages. Currently: `Presence.SocialFormat.Lib` | `nuget`          |

## Publication to Nuget

Library packages are published to Nuget.

Create a [release](https://github.com/instantiator/presence/releases) from the main branch, specifying the latest tag you have applied, to trigger build and publication of packages to Nuget.
