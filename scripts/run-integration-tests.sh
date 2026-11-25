#!/usr/bin/env bash
set -euo pipefail

DEFAULT_ENV_PATH=.env.accounts.integration

usage() {
  cat << EOF

Runs the integration tests. These post to the configured social network accounts.

Options:
    -f <filter>    --filter <filter>               Filter the tests by name (optional)
    -e <path>      --env-file <path>               Path to an environment file (optional, default: $DEFAULT_ENV_PATH if present)
    -h             --help                          Prints this help message and exits

EOF
}

ENV_PATH=""
FILTER=""
while [ $# -gt 0 ]; do
  case $1 in
  -e | --env-file)
    shift
    ENV_PATH=${1:?--env-file needs a path}
    ;;
  -f | --filter)
    shift
    FILTER=${1:?--filter needs a value}
    ;;
  -h | --help)
    usage
    exit 0
    ;;
  *)
    echo "Unknown option $1..."
    usage
    exit 1
    ;;
  esac
  shift
done

# Without -e, use the local accounts file if there is one; otherwise (eg. CI) the environment must hold the config
if [ -z "$ENV_PATH" ] && [ -f "$DEFAULT_ENV_PATH" ]; then
  ENV_PATH=$DEFAULT_ENV_PATH
fi

if [ -n "$ENV_PATH" ]; then
  if [ ! -f "$ENV_PATH" ]; then
    echo "Environment file not found: $ENV_PATH"
    usage
    exit 1
  fi
  echo "Environment config path: $ENV_PATH"
  echo
  set -o allexport
  # shellcheck source=/dev/null
  source "$ENV_PATH"
  set +o allexport
fi

TEST_FILTER="TestCategory=Integration"
if [ -n "$FILTER" ]; then
  echo "Running integration tests with filter: $FILTER"
  TEST_FILTER="$TEST_FILTER&FullyQualifiedName~$FILTER"
else
  echo "Running all integration tests..."
fi
echo

# test.runsettings excludes integration tests by default; clear it so this filter applies alone
dotnet test --verbosity normal -p:RunSettingsFilePath= --filter "$TEST_FILTER"
