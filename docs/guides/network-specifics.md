# Network specific configuration

## Configuration

`Presence.Posting.Console` accepts configuration variables through the environment or from a `.env` format configuration file.

Here is a a sample `.env` configuration file, each part is explained below:

```env
PRESENCE_ACCOUNTS="TEST0,TEST1"
TEST0_Console_PrintPrefix="Console>"
TEST1_AT_AccountName="presence-lib-test.bsky.social"
TEST1_AT_AppPassword="<the app password goes here>"
```

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

These combined to give a key of: `TEST1_AT_AccountName`

## Networks

Each network requires specific configuration keys.

## `Console` network

The Console network is used to test threads by printing to your console instead of posting to a social network.

The Network is always: `Console`

| Credential type | Req      | Value                                                         |
| --------------- | -------- | ------------------------------------------------------------- |
| `PrintPrefix`   | Required | A short piece of text to print at the beginning of each line. |

eg. The following line defines the `PrintPrefix` key and value in a `.env` configuration file:

```env
TEST0_Console_PrintPrefix="Console>"
```

## `AT` (BlueSky) network

Create an application password for your account in **Privacy & Security** settings in BlueSky.

The Network is always: `AT`

| Credential type | Req      | Value                                                     |
| --------------- | -------- | --------------------------------------------------------- |
| `AccountName`   | Required | Your account name, eg. `instantiator.bsky.social`         |
| `AppPassword`   | Required | An application password created in your account           |
| `Server`        |          | The AT network server to connect to, if not `bsky.social` |

eg. The following line defines the `AccountName` variable for an `AT` account with prefix `TEST1`, in the format of a `.env` configuration file:

```env
TEST1_AT_AccountName="presence-lib-test.bsky.social"
```

You must provide both `AccountName` and `AppPassword` to be able to connect to an AT network.

The `Server` value is optional, and can be used to indicate the AT service the account is with, if it is not `bsky.social`.
