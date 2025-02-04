# Using `Presence.Posting.Console`

## Installation

You can either [install the CLI tools](install-cli-tools.md) for your system from the latest release, or build and run the latest version of the code from the source repository.

To build and run from the repository, clone the repository (if you haven't already), and use the `post.sh` script. (You'll need to have .NET 8 installed.)

Examples in this document use installed binaries, ie. `Presence.Posting.Console`. You can substitute `./post.sh` to invoke the post script if needed.

## Usage

The prebuilt binary is either `Presence.Posting.Console` (Mac OS or Linux), or `Presence.Posting.Console.exe` (Windows).

- Use the `--help` option for more information about accepted parameters

This tool is commonly used with `Presence.SocialFormat.Console` which can provide formatted thread data for posting. Both tools support the `--help` option.

## Inputs

- Provide input as a JSON-formatted `ThreadCompositionResponse`
  - Provide a path to a file with the `-f` / `--input-file` parameter, or through `stdin`
- Social network configuration as environment variables
  - Provide a path to a `.env` file with the `-e` / `--env-file` parameter, or in the environment

## Output

The tool will attempt to post each thread to its social network, as specified in the input.

The output is a summary of all posting activity, as a JSON-formatted [`ThreadPostingResponse`](https://github.com/instantiator/presence/tree/main/Presence.Posting.Console/DTO/ThreadPostingResponse.cs)

## Examples

As mentioned above, these examples use the pre-built binaries.

### Format and post from a markdown source

This example uses `Presence.SocialFormat.Console` to format a thread for an AT network (eg. BlueSky) from a markdown file, and then pipes it into `Presence.Posting.Console` to send to the network.

```bash
Presence.SocialFormat.Console -f SampleData/SimpleThread.md -n AT | Presence.Posting.Console -e .env.networks
```

#### Notes for `Presence.SocialFormat.Console`

- the `-o JSON` option is not included - this is the default output type
- the `-n AT` option indicates that the content should be formatted for an AT network (eg. BlueSky)
- the `-i MD` input type (a simplified Markdown-like format) is determined from the filename

#### Notes for `Presence.Posting.Console`

- the required input type input type is a JSON formatted `ThreadCompositionResponse` (as provided through the pipe)
- the `-e .env.networks` option sources [network configuration](../guides/network-specifics.md) (ie. account credentials) from `.env.networks`

### Pipe a simple post into the chain

This example is similar, illustrating how you can provide content very simply without having to create a separate file. In this case, `echo` is used to pipe a little markdown text into the formatter.

```bash
echo "Romani ite domum" | Presence.SocialFormat.Console -i MD -n AT | Presence.Posting.Console -e .env.networks
```

#### Notes for `Presence.SocialFormat.Console`

- the `-i MD` option is required, as the formatter cannot guess the format of input from `stdin`
