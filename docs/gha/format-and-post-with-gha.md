# Format and post a thread with GitHub Actions

This process is split into two steps, each served by a different GitHub Action:

1. `instantiator/presence/format-thread` - formats thread content for each social network specified
2. `instantiator/presence/post-thread` - posts a formatted thread to social networks specified (using credentials specified)

Output from the first step is a fully formatted set of threads, one for each network type. This can then be passed into the second step as an input alongside social network credentials to send the posts.

For a worked example, see the sample workflow found in:

* [`on-push-test-actions.yml`](https://github.com/instantiator/presence/blob/main/.github/workflows/on-push-test-actions.yml)

## Formatting

Use the `instantiator/presence/format-thread` action:

```yaml
- uses: actions/checkout@v4
- name: Format sample thread
  id: format-thread
  uses: instantiator/presence/format-thread@main
  with:
    networks: 'Console,AT'
    input-file: 'SampleData/SimpleThread.md'
```

In this example, the formatter prepares a thread found in a file: `SimpleThread.md`

The `id` field is essential if you wish to pass the output from this step on, eg. as `${{ steps.format-thread.outputs.format-response }}`

Here the inputs, under `with:` provide key information:

* `networks:` - a comma-separated list of networks to format a content for
* `input-file:` - a source file providing the input content (here, it's in 'markdown-like' syntax)

Alternatively, provide content directly rather than from a file:

```yaml
    input-format: 'MD'
    input-content: |
      This is a simple thread.

      With two posts.
```

`input-content` and `input-file` are mutually exclusive. `input-format` is required if `input-content` is provided.

For information about preparing content, see:

* [Create threads with markdown](../guides/create-with-markdown.md)

## Posting

Use the `instantiator/presence/post-thread` action:

```yaml
- name: Post sample thread to console
  id: post-thread
  uses: instantiator/presence/post-thread@main
  with:
    thread: ${{ steps.format-thread.outputs.format-response }}
```

The `id` field is optional, but helpful if you want to refer to its output later, eg. as `${{ steps.post-thread.outputs.post-thread-summary }}`

This step will also need some configuration to tell it about network credentials...

### Configuration options

You should provide credentials for all networks you intend to post to.

It is recommended to store your social network credentials in GitHub Actions Secrets, to prevent their being discovered / abused.

For information about configuration keys, see: [Network specific configuration](../guides/network-specifics.md)

NB. If you have not provided a thread for each social network configured, the posts that can be sent will be sent, but the step will report failure.

There are several ways to provide configuration:

1. Provide all secrets as JSON _(simplest option)_
2. Craft the JSON configuration _(better control of your secrets)_
3. As environment variables _(probably easiest to read)_

#### 1. Provide all secrets as JSON _(simplest option)_

If you're confident you have no other important secrets in your repository, it's simplest to provide all your secrets as a JSON object.

This is the approach shown in: [`on-push-test-actions.yml`](https://github.com/instantiator/presence/blob/main/.github/workflows/on-push-test-actions.yml)

It's also the option that will require the least maintenance if you intend to add more credentials/secrets in future.

```yaml
with:
    thread: ${{ steps.format-thread.outputs.format-response }}
    config: ${{ toJson(secrets) }}
```

#### 2. Craft the JSON configuration _(better control of your secrets)_


Provide a single `config` input containing a JSON dictionary of all configuration.

This is a little more complex, but allows you much better control of which information is passed to the action.

```yaml
with:
    thread: ${{ steps.format-thread.outputs.format-response }}
    config: "{ \"PRESENCE_ACCOUNTS\": \"TEST0,TEST1\", \"TEST0_CONSOLE_PRINTPREFIX\": \"[TEST0-GHA]\", \"TEST1_AT_ACCOUNTNAME\": \"${{ secrets.TEST1_AT_ACCOUNTNAME }}\", \"TEST1_AT_APPPASSWORD\": \"${{ secrets.TEST1_AT_APPPASSWORD }}\" }"
```

#### 3. Provide environment variables _(probably easiest to read)_

Pass individual secrets as environment variables.

Similarly, this grants you item-by-item control of which information is passed to the action.

```yaml
with:
    thread: ${{ steps.format-thread.outputs.format-response }}
env:
    PRESENCE_ACCOUNTS: "TEST0,TEST1"
    TEST0_CONSOLE_PRINTPREFIX: "[TEST0-GHA]"
    TEST1_AT_ACCOUNTNAME: ${{ secrets.TEST1_AT_ACCOUNTNAME }}
    TEST1_AT_APPPASSWORD: ${{ secrets.TEST1_AT_APPPASSWORD }}
```