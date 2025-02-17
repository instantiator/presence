# Set up an `AT` network

`AT` is the network type for BlueSky and similar social networks.

## Setting up a connection

Create an application password for your account in **Privacy & Security** settings in BlueSky.

The Network part of each configuration key is always: `AT`

## Network credentials

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

For examples, see: [Network specific configuration](../network-specifics.md)