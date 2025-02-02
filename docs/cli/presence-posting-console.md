# Using `Presence.Posting.Console`

## Installation

You can either [download the binaries](download-binaries.md) for your system from the latest release, or build and run the latest version of the code from this repository.

To build and run from the repository, clone the repository (if you haven't already), and use the `post.sh` script. (You'll need to have .NET 8 installed.)

Examples in this document use the prebuilt binary, ie. `Presence.Posting.Console`. You can substitute the `post.sh` script if needed.

## Usage

The prebuilt binary is either `Presence.Posting.Console` (Mac OS or Linux), or `Presence.Posting.Console.exe` (Windows).

- Provide the `--help` parameter for more information

This tool is commonly used with `Presence.SocialFormat.Console` which can provide formatted thread data for posting.

## Inputs

- Provide input as a JSON-formatted `ThreadCompositionResponse`
  - Provide a path to a file with the `-f` / `--input-file` parameter, or through `stdin`
- Social network configuration as environment variables
  - Provide a path to a `.env` file with the `-e` / `--env-file` parameter, or in the environment

## Output

The tool will attempt to post each thread to its social network, as specified in the input.

The output is a summary of all posting activity, as a JSON-formatted [`ThreadPostingResponse`](https://github.com/instantiator/presence/tree/main/Presence.Posting.Console/DTO/ThreadPostingResponse.cs)

## Examples

As mentioned above, these examples use the pre-built binaries, but you may substitute the scripts (`format.sh` and `post.sh`) to build and run from the repository code (with the .NET 8 SDK installed)

```bash
./Presence.SocialFormat.Console -f SampleData/SimpleThread.md -n AT -o JSON | ./Presence.Posting.Console -e .env.integration
```
