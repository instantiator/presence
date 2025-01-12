#!/bin/bash

usage() {
  cat << EOF

Runs the unit tests.

Options:
    -h             --help                          Prints this help message and exits

EOF
}

# defaults

# parameters
while [ -n "$1" ]; do
  case $1 in
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

dotnet test --verbosity normal --filter TestCategory=Unit "$@"
