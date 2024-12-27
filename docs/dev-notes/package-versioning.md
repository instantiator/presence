# Package versioning

To version a package, first push an appropriate tag, eg.

```bash
git tag social-format-lib-0.1.0
git push --tags
```

NB. `Presence.SocialFormat.Lib` version tags are prefixed with: `social-format-lib-`

Create a [release](https://github.com/instantiator/presence/releases) from the main branch to trigger build and publication of the package to Nuget.

| Package            | Tag prefix           | Nuget location                                                       |
| ------------------ | -------------------- | -------------------------------------------------------------------- |
| `Presence.SocialFormat.Lib` | `social-format-lib-` | [Presence.SocialFormat.Lib](https://www.nuget.org/packages/Presence.SocialFormat.Lib/) |
