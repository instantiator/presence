#!/bin/bash

usage() {
  cat << EOF

Runs the unit tests.

Options:
    -f <filter>    --filter <filter>               Filter the tests by name (optional)
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
  -f | --filter)
    shift
    FILTER=$1
    ;;
  *)
    echo "Unknown option $1..."
    usage
    exit 1
    ;;
  esac
  shift
done

if [ ! -z "$FILTER" ]; then
  echo "Running unit tests with filter: $FILTER"
  echo
  dotnet test --verbosity normal --filter "TestCategory=Unit&FullyQualifiedName~$FILTER"
else
  echo "Running all unit tests..."
  echo
  dotnet test --verbosity normal --filter TestCategory=Unit
fi
