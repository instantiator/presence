# Set up a `Console` network

The Console network is used to test threads by printing to your console instead of posting to a social network.

The Network part of each configuration key is always: `Console`

## Network credentials

| Credential type | Req      | Value                                                         |
| --------------- | -------- | ------------------------------------------------------------- |
| `PrintPrefix`   | Required | A short piece of text to print at the beginning of each line. |

eg. The following line defines the `PrintPrefix` key and value in a `.env` configuration file:

```env
TEST0_Console_PrintPrefix="Console>"
```

For examples, see: [Network specific configuration](../network-specifics.md)