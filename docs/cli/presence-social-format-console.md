# Using `Presence.SocialFormat.Console`

## Installation

You can either [install the CLI tools](install-cli-tools.md) for your system from the latest release, or build and run the latest version of the code from the source code repository.

To build and run from the repository, clone the repository (if you haven't already), and use the `format.sh` script. (You'll need to have .NET 8 installed.)

Examples in this document use installed binaries, ie. `Presence.SocialFormat.Console`. You can substitute `./format.sh` to invoke the post script if needed.

## Usage

The prebuilt binary is either `Presence.SocialFormat.Console` (Mac OS or Linux), or `Presence.SocialFormat.Console.exe` (Windows).

- Provide the `--help` parameter for more information

### Inputs

- Inputs accepted as either a file (as an `-f` / `--input-file`) parameter, or through `stdin`
- Input formats may be specified with the `-i` / `--input-format` parameter
- Input formats accepted are:
  - `-i JSON` - a JSON-formatted [`ThreadCompositionRequest`](https://github.com/instantiator/presence/blob/main/Presence.SocialFormat.Lib/DTO/ThreadCompositionRequest.cs) describing the content
  - `-i MD` - a [pseudo-markdown](https://instantiator.dev/presence/guides/create-with-markdown.html) formatted file describing the content
- Networks may be specified with the `-n` / `--networks` parameter
- Provide networks as a comma-separated list of network codes
- Supported networks are listed in the `--help` text, or see: [Network specific configuration](../guides/network-specifics.md)

Examples of supported networks are:

- `Console` - the 'Console' network (prints to console, useful for testing)
- `AT` - an AT-network (ie. BlueSky)

### Outputs

- The output format may also be specified with the `-o` / `--output-format` parameter.
- Output formats accepted are:
  - `-o JSON` - a JSON-formatted [`ThreadCompositionResponse`](https://github.com/instantiator/presence/blob/main/Presence.SocialFormat.Lib/DTO/ThreadCompositionResponse.cs)
  - `-o MD` - pseudo-markdown format
  - `-o HR` - a friendlier human-readable format (useful for inspecting output)

NB. JSON is most useful if you intend to post using `Presence.Posting.Console`

See also:

* [Network specific constraints](../guides/network-constraints.md)
* [SampleData](https://github.com/instantiator/presence/tree/main/SampleData)

## Examples

_(These examples assume that the pre-built binary is available in the working directory.)_

To format the sample thread for BlueSky (AT), you can pass in the request json as a file:

```bash
Presence.SocialFormat.Console -f SampleData/simple-snippets.json -n AT
```

Or pipe it in through `stdin`:

```bash
cat SampleData/simple-snippets.json | Presence.SocialFormat.Console -n AT
```

## Input formats

Provide input as either:

* a JSON-formatted [`ThreadCompositionRequest`](https://github.com/instantiator/presence/blob/main/Presence.SocialFormat.Lib/DTO/ThreadCompositionRequest.cs), or
* a markdown-like representation of content

See: [Create threads with markdown](../guides/create-with-markdown.md)

### Example

The `SampleData/` directory contains some examples of each.

```bash
Presence.SocialFormat.Console -f SampleData/SimpleThread.md -n Console,AT -o HR
```

- `-f SampleData/SimpleThread.md` - use the markdown-like sample input file
- `-n Console,AT` - format threads for the Console (test), and AT (BlueSky) networks
- `-o HR` - render the results in a simple, human readable format

Result:

```text
Network: Console#0 ✅
---
 ⏩ 1. Hello from Presence! Presence is a social network
#Presence #CSharp #SocialNetwork #Posting #Tool
   ---
 ⏩ 2. thread formatting and posting tool.
#Presence #CSharp #SocialNetwork #Posting #Tool
   ---
 ⏩ 3. Find out more about Presence, the social
#Presence #CSharp #SocialNetwork #Posting #Tool
   ---
 ⏩ 4. formatting library, at:
#Presence #CSharp #SocialNetwork #Posting #Tool
   ---
 ⏩ 5. https://instantiator.dev/presence
#Presence #CSharp #SocialNetwork #Posting #Tool

Network: AT#0 ✅
---
 ⏩ 1. Hello from Presence! Presence is a social network thread formatting and posting tool. #Presence #CSharp #SocialNetwork #Posting #Tool
   ---
 ⏩ 2. Find out more about Presence, the social formatting library, at: https://instantiator.dev/presence
```
