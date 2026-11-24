# Testing

A number of unit tests and integration tests are provided in the `Presence.SocialFormat.Lib.Tests` and `Presence.Posting.Lib.Tests` projects.

Tests are categorised across all projects using the `TestCategory` attribute - with these values:

- `Unit`
- `Integration`

## Test scripts

Use the test scripts to run tests locally.

| Script                             | Purpose                                        |
| ---------------------------------- | ---------------------------------------------- |
| `scripts/run-unit-tests.sh`        | Run all unit tests across the solution.        |
| `scripts/run-integration-tests.sh` | Run all integration tests across the solution. |

Both scripts accept an `-f` / `--filter` parameter to allow you to filter the tests that will run by name.

A plain `dotnet test` runs the unit tests only. `test.runsettings` (wired in through `Directory.Build.props`) excludes `Integration` tests, because they post to real accounts. `scripts/run-integration-tests.sh` clears that setting to run them.

## Dependencies

Please note that both the unit tests and integration tests assume internet access.

This is, perhaps, unusual. Unit tests are often assumed to be able to run independently and in full isolation. However, some of the unit tests establish that code to access internet services works and this is considered more 'primitive' than integrations with third party services, such as social networks. You may disagree.

The integration tests also assume access to the supported social networks (and sufficient configuration to run against a test account). See [Network specifics](../guides/network-specifics.md) for more information about configuration per network.

## Integration testing

The integration tests connect to third party social network services, and test the ability to connect and post.

### Configuration

The integration tests require configuration containing account credentials for the various social network accounts they must connect to.

Configuration can be provided through a `.env` file or through environment variables. Either method requires the same key/value pairs.

For information about configuration variables, see: [Network specifics](../guides/network-specifics.md)

Provide the path to a configuration file with the `-e` / `--env-file` parameter to `scripts/run-integration-tests.sh`. Without it, the script uses `.env.accounts.integration` if that file exists, and otherwise expects the variables to be in the environment.

```bash
./scripts/run-integration-tests.sh -e .env.accounts.integration
```

Configuration parameters are stored in GitHub Secrets and provided to the GitHub Actions integration tests as environment variables.

### Accounts

A number of accounts exist for integration testing across social networks.

| Social network | Account                                                                                 |
| -------------- | --------------------------------------------------------------------------------------- |
| BlueSky        | [presence-lib-test.bsky.social](https://bsky.app/profile/presence-lib-test.bsky.social) |
