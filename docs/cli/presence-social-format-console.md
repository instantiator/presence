# Using `Presence.SocialFormat.Console`

## Installation

You can either [download the binaries](download-binaries.md) for your system from the latest release, or build and run the latest version of the code from this repository.

To build and run from the repository, clone the repository (if you haven't already), and use the `format.sh` script. (You'll need to have .NET 8 installed.)

Examples in this document use `format.sh` - you can substitute this for a path to the appropriate binary, eg. `Presence.SocialFormat.Console`

## Sample usage

To format the sample thread for BlueSky (AT), you can pass in the request json as a file:

```bash
./format.sh -i SampleData/simple-snippets.json -n AT
```

Or pipe it in through `stdin`:

```bash
cat SampleData/simple-snippets.json | ./format.sh -n AT
