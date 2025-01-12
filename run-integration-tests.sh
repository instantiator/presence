#!/bin/bash

usage() {
  cat << EOF

Runs the integration tests.

Options:
    -e <path>      --env-file <path>               Path to an environment file (eg. .env.integration)
    -h             --help                          Prints this help message and exits

EOF
}

# defaults

# parameters
while [ -n "$1" ]; do
  case $1 in
  -e | --env-file)
    shift
    ENV_PATH=$1
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

# if ENV_PATH is provided - source environment variables from it
if [ ! -z "$ENV_PATH" ]; then
  if [ ! -f "$ENV_PATH" ]; then
    echo "Environment file not found: $ENV_PATH"
    usage
    exit 1
  else
    echo "Environment config path: $ENV_PATH"
    echo
    set -o allexport
    source $ENV_PATH
    set +o allexport
  fi
fi

dotnet test --verbosity normal --filter TestCategory=Integration
