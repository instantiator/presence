# Block Kit (Slack)

Some notes on the generation of types (as C# classes) to work with [Block Kit](https://api.slack.com/block-kit), Slack's rather interesting, interactive post-and-UI toolkit.

## Schema

To generate C# classes, we first need a schema.

[This issue](https://github.com/slackapi/slack-api-specs/issues/18) discusses a request for a schema from Slack. It's unfulfilled, but [this comment](https://github.com/slackapi/slack-api-specs/issues/18#issuecomment-1756206605), by [Lawrence Gripper](https://github.com/lawrencegripper) links out to a [gist of the schema](https://gist.github.com/lawrencegripper/ee24242249fc6a1a0a73c561766dd486) that he generated himself.

## Generating types

[`Corvus.JsonSchema`](https://github.com/corvus-dotnet/Corvus.JsonSchema) features a library and toolset for working with JSON schemas. The `generatejsonschematypes` CLI tool can be used to generate classes for the various types found in a schema (along with seralization, deserialization and validation methods).

The process is fairly simple:

1. [Install the tool](https://api.slack.com/block-kit)
1. Place the schema in `Presence.Posting.Lib/Connections/Slack/BlockKit/Generated`
1. Generate the classes, explicitly stating the namespace, root object to generate, and path to the schema.
1. Install the [`Corvus.Json.ExtendedTypes`](https://www.nuget.org/packages/Corvus.Json.ExtendedTypes/) nuget package.

  ```bash
  generatejsonschematypes --rootNamespace Presence.Posting.Lib.Connections.Slack.BlockKit --rootPath "#/" Presence.Posting.Lib/Connections/Slack/BlockKit/Generated/blockkit.schema.json
  ```

By default, classes will be generated in the same directory as the schema.

## Output

The [hands-on lab](https://github.com/corvus-dotnet/Corvus.JsonSchema/blob/main/docs/GettingStartedWithJsonSchemaCodeGeneration.md) documentation leans heavily into serializing and deserializing JSON strings, but [it also supports use of anonymous types](https://github.com/corvus-dotnet/Corvus.JsonSchema/blob/main/docs/GettingStartedWithJsonSchemaCodeGeneration.md#using-anonymous-types) to provide data for new objects.

[There are also](https://github.com/corvus-dotnet/Corvus.JsonSchema/blob/main/docs/GettingStartedWithJsonSchemaCodeGeneration.md#using-create-to-create-objects) `Create()` methods assigned to each type that allow for explicit, programmatic construction of objects.

## Other tools

Notes on some other, related, JSON and JSON-schema related tooling for .NET application development...

## quicktype.io

[quicktype.io](https://app.quicktype.io/?l=csharp) looks nice - and it purports to do the right thing. In this case, though, it did not succeed in creating all the types required from the given schema. It generates the outermost class with a single `type` field, but none of the more specific and conditional types described in `$.items.allOf`

## `json2csharp`

https://json2csharp.com/

This tool looks like it can generate C# classes from JSON objects - which is a valuable approach - although here, we'd prefer to build from a well-defined schema.

## `JsonSchema.NET`

https://github.com/RicoSuter/NJsonSchema

This looks like a nice code library for working with JSON schemas - either generating them or using them to validate data.

