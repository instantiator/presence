# Network specific configuration

All configuration to Presence is provided as key-value pairs, through environment variables, a `.env` formatted file, or in a JSON-formatted command-line parameter.

For information about each network's limitations, see: [Network specific constraints](network-constraints.md)

## Accounts

Presence supports multiple accounts.

Assign a unique prefix to each account to help you distinguish between them, and provide a comma-separated list of account prefixes in the `PRESENCE_ACCOUNTS` variable.

eg. In this example, there are 2 accounts, with prefixes `TEST0` and `TEST1`:

```env
PRESENCE_ACCOUNTS="TEST0,TEST1"
```

You will likely assign much more meaningful names to your accounts.

## Configuration keys

Each configuration key is constructed as follows:

```
<AccountPrefix>_<Network>_<CredentialType>
```

For example:

- The account prefix is: `TEST1`
- The network is: `AT` (ie. BlueSky)
- The credential type is: `AccountName`
- Each part of the key is separated by an `_` underscore character

These combine to give a key of: `TEST1_AT_AccountName`

## Networks

Each network requires specific configuration keys, which relate to the configuration required to connect to and post to the network.

See:

* [Set up a `Console` network](networks/setup-console.md)
* [Set up an `AT` network](networks/setup-at.md)
* [Set up a `Slack` network](networks/setup-slack.md)

## Configuration by variables

`Presence.Posting.Console` accepts configuration variables through the environment or from a `.env` format configuration file.

Filename: `.env.example`

```env
PRESENCE_ACCOUNTS="TEST0,TEST1"
TEST0_Console_PrintPrefix="Console>"
TEST1_AT_AccountName="presence-lib-test.bsky.social"
TEST1_AT_AppPassword="<the app password goes here>"
```

### Example 1: Environment variables

```bash
Presence.SocialFormat.Console -f SampleData/SimpleThread.md -n AT,Console | Presence.Posting.Console
```

### Example 2: `-e` / `--env-file` parameter

```bash
Presence.SocialFormat.Console -f SampleData/SimpleThread.md -n AT,Console | Presence.Posting.Console -e .env.example
```

## Configuration by JSON

Alternatively, you may provide the configuration as a json object that's effectively an `IDictionary<string,string>`

You may provide this to `Presence.Posting.Console` either:

* as the `PRESENCE_CONFIG_JSON` variable (see: `ConfigKeys`), or
* in the `-j` / `--json-config` parameter

This key supercedes any others found in the configuration.

### Example 1: `PRESENCE_CONFIG_JSON` as an environment variable

Sample JSON:

```json
{ "PRESENCE_ACCOUNTS": "TEST0", "TEST0_Console_PrintPrefix": "JsonConfiguredConsole" }
```

provide configuration JSON in the `PRESENCE_CONFIG_JSON` variable, and invoke with:

```bash
Presence.SocialFormat.Console -f SampleData/SimpleThread.md -n Console | Presence.Posting.Console
```

### Example 2: `PRESENCE_CONFIG_JSON` variable in a `.env` file

Filename: `.env.example`

```env
PRESENCE_JSON_CONFIG="{ \"PRESENCE_ACCOUNTS\": \"TEST0\", \"TEST0_Console_PrintPrefix\": \"JsonConfiguredConsole\" }"
```

Invoke with:

```bash
Presence.SocialFormat.Console -f SampleData/SimpleThread.md -n Console | Presence.Posting.Console -e .env.example
```

In this case, **only** the `PRESENCE_CONFIG_JSON` key will be used from: `.env.example`

* The console prefix will be: `JsonConfiguredConsole`
* The other definition of `TEST0_Console_PrintPrefix` is ignored
* Any other variables provided in the `.env` file would also be ignored

### Example 3: `-j` / `--json-config` parameter

Invoke with:

```bash
Presence.SocialFormat.Console -f SampleData/SimpleThread.md -n Console | Presence.Posting.Console -j "{ \"PRESENCE_ACCOUNTS\": \"TEST0\", \"TEST0_Console_PrintPrefix\": \"JsonConfiguredConsole\" }"
```

In the example above, all environment variables are ignored, and configuration comes only from the `-j` / `--json-config` parameter.