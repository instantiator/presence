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

- Provide input to `Presence.Posting.Console` as a JSON-formatted [`ThreadCompositionResponse`](https://github.com/instantiator/presence/blob/main/Presence.SocialFormat.Lib/DTO/ThreadCompositionResponse.cs)
  - Provide a path to a file with the `-f` / `--input-file` parameter, or through `stdin`
- Provide configuration with credentials for social networks as:
  - environment variables, or
  - a path to a `.env` file, with the `-e` / `--env-file` parameter, or
  - a JSON dictionary object, with the `-j` / `--json-config` parameter

See also: [Network specific configuration](../guides/network-specifics.md)

## Output

The tool will attempt to post each thread to its social network, as specified in the input.

The output is a summary of all posting activity, as a JSON-formatted [`ThreadPostingResponse`](https://github.com/instantiator/presence/tree/main/Presence.Posting.Console/DTO/ThreadPostingResponse.cs)

For example, this invocation uses `PresenceSocialFormat.Console` to generate a thread for the `AT` network, and pipes it to `Presence.Posting.Console` with configuration in `.env.accounts.integration` which specifies two accounts: `TEST0` for the `Console` network, and `TEST1` with credentials for an `AT` network

```bash
Presence.SocialFormat.Console -f SampleData/SimpleThread.md -n AT | Presence.Posting.Console -e .env.accounts.integration
```

_For more information about network configuration, see: [Network specific configuration](../guides/network-specifics.md)_

This generates the following result:

```json
{
  "FullSuccess": false,
  "Summaries": {
    "TEST0": {
      "Console": {
        "AccountPrefix": "TEST0",
        "Network": "Console",
        "Success": false,
        "PostReferences": null,
        "Posts": null,
        "ExceptionType": "KeyNotFoundException",
        "ExceptionMessage": "0 threads found for network Console"
      }
    },
    "TEST1": {
      "AT": {
        "AccountPrefix": "TEST1",
        "Network": "AT",
        "Success": true,
        "PostReferences": [
          {
            "uri": "at://did:plc:twi7ciayuwafky5ryxofokq5/app.bsky.feed.post/3lhjsjdsvnz2p",
            "did": "did:plc:twi7ciayuwafky5ryxofokq5",
            "cid": "bafyreif34mn2m4i3bud7g7hqbjxy5o7quzjoxm6wl3ih7wd2sp27lbpt7e",
            "rkey": "3lhjsjdsvnz2p",
            "link": "https://bsky.app/profile/presence-lib-test.bsky.social/post/3lhjsjdsvnz2p"
          },
          {
            "uri": "at://did:plc:twi7ciayuwafky5ryxofokq5/app.bsky.feed.post/3lhjsjeqqer2n",
            "did": "did:plc:twi7ciayuwafky5ryxofokq5",
            "cid": "bafyreibqwkzpqauowqersvqdje5zeaitgwv4fjm7pscw4c7ym3jar6urla",
            "rkey": "3lhjsjeqqer2n",
            "link": "https://bsky.app/profile/presence-lib-test.bsky.social/post/3lhjsjeqqer2n"
          }
        ],
        "Posts": 2,
        "ExceptionType": null,
        "ExceptionMessage": null
      }
    }
  }
}
```

In this example, the `TEST0` account could not post - because the formatter did not provide a thread formatted for the `Console` network. `TEST1` succeeded - and posted its thread to the `AT` network. Information about each post is included in the `PostReferences` field, and the `Posts` field indicate that the thread contains 2 posts.

## Examples

As mentioned above, these examples use the pre-built binaries.

### Format and post from a markdown source

This example uses `Presence.SocialFormat.Console` to format a thread for an AT network (eg. BlueSky) from a markdown file, and then pipes it into `Presence.Posting.Console` to send to the network.

```bash
Presence.SocialFormat.Console -f SampleData/SimpleThread.md -n AT | Presence.Posting.Console -e .env.accounts
```

#### Notes for `Presence.SocialFormat.Console`

- the `-o JSON` option is not included - this is the default output type
- the `-n AT` option indicates that the content should be formatted for an AT network (eg. BlueSky)
- the `-i MD` input type (a simplified Markdown-like format) was determined from the filename

#### Notes for `Presence.Posting.Console`

- the required input type input type is a JSON formatted `ThreadCompositionResponse` (as provided through the pipe)
- the `-e .env.accounts` option sources [network configuration](../guides/network-specifics.md) (ie. account credentials) from `.env.accounts`

### Pipe a simple post into the chain

This example is similar, illustrating how you can provide content very simply without having to create a separate file. In this case, `echo` is used to pipe a little markdown text into the formatter.

```bash
echo "Romani ite domum" | Presence.SocialFormat.Console -i MD -n AT | Presence.Posting.Console -e .env.accounts
```

#### Notes for `Presence.SocialFormat.Console`

- the `-i MD` option is required, as the formatter cannot guess the format of input from `stdin`
