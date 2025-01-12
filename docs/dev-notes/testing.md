# Testing

A number of unit tests and integration tests are provided in the `Presence.SocialFormat.Tests` and `Presence.Posting.Tests` projects.

Tests are categorised across all projects using the `TestCategory` attribute - with these values:

- `Unit`
- `Integration`

## Test scripts

Use the test scripts to run tests locally.

| Script                     | Purpose                                        |
| -------------------------- | ---------------------------------------------- |
| `run-unit-tests.sh`        | Run all unit tests across the solution.        |
| `run-integration-tests.sh` | Run all integration tests across the solution. |

Optionally, provide the path to a configuration file as the first parameter to `run-integration-tests.sh`, eg.

```bash
./run-integration-tests.sh .env.integration
```

If this path is not provided, configuration is expected to be made available through environment variables.

## Integration testing

The integration tests connect to third party social network services, and test the ability to connect and post.

### Configuration

The integration tests require configuration containing account credentials for the various social network accounts they must connect to.

Configuration can be provided through a `.env` file or through environment variables. Either method requires the same key/value pairs.

Configuration parameters are stored in GitHub Secrets and provided to the GitHub Actions integration tests as environment variables.

For config values, see: [Network specifics](../guides/network-specifics.md)

### Accounts

A number of accounts exist for integration testing across social networks.

| Social network | Account                                                                                 |
| -------------- | --------------------------------------------------------------------------------------- |
| BlueSky        | [presence-lib-test.bsky.social](https://bsky.app/profile/presence-lib-test.bsky.social) |
