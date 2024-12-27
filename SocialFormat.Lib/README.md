# SocialFormat.Lib

This library will format a thread of posts for a given social network from a set of snippets (text, links, hashtags, breaks).

- Provides the `CommonPost` and `SocialSnippet` classes - used to assemble messages that can be formatted for distribution across any social network.
- Provides thread composition classes, and the `IThreadComposer` interface for interacting with them. Threads are formatted into `CommonPosts` from `SocialSnippets`.

## Getting started

**TODO - package installation instructions**

## Create a thread

**TODO - thread creation with an IThreadComposer**

## Dependencies

| Dependency                                                                                                                                                        | Creator                                      | License                                                                                    | Usage                        |
| ----------------------------------------------------------------------------------------------------------------------------------------------------------------- | -------------------------------------------- | ------------------------------------------------------------------------------------------ | ---------------------------- |
| [MinVer](https://github.com/adamralph/minver)                                                                                                                     | [Adam Ralph](https://github.com/adamralph)   | [Apache 2.0](https://github.com/adamralph/minver?tab=Apache-2.0-1-ov-file#readme)          | Manage package versions      |
| [Dotnet.ReproducibleBuilds](https://github.com/dotnet/reproducible-builds)                                                                                        | [.NET Platform](https://github.com/dotnet)   | [MIT](https://github.com/dotnet/reproducible-builds?tab=MIT-1-ov-file#readme)              | Simplify build configuration |
| [Meziantou.Framework.NuGetPackageValidation.Tool](https://github.com/meziantou/Meziantou.Framework/blob/main/src/Meziantou.Framework.NuGetPackageValidation.Tool) | [Gérald Barré](https://github.com/meziantou) | [MIT](https://github.com/meziantou/Meziantou.Framework/tree/main?tab=MIT-1-ov-file#readme) | Validate package properties  |
