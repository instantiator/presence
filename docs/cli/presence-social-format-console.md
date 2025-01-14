# Using `Presence.SocialFormat.Console`

## Installation

You can either [download the binaries](download-binaries.md) for your system from the latest release, or build and run the latest version of the code from this repository.

To build and run from the repository, clone the repository (if you haven't already), and use the `format.sh` script. (You'll need to have .NET 8 installed.)

Examples in this document use `format.sh` - you can substitute this for a path to the appropriate binary, eg. `Presence.SocialFormat.Console`

## Options

Run with no parameters, or the `--help` option to see the help information:

```bash
./format.sh --help
```

## Sample usage

To format the sample thread for BlueSky (AT), you can pass in the request json as a file:

```bash
./format.sh -f SampleData/simple-snippets.json -n AT
```

Or pipe it in through `stdin`:

```bash
cat SampleData/simple-snippets.json | ./format.sh -n AT
```

## Input formats

Presence can parse a `ThreadCompositionRequest` formatted as JSON, or a markdown-like representation of content. The `SampleData/` directory contains some examples of each.

```bash
./format.sh -f SampleData/SimpleThread.md -n Console,AT -o HR
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
