#!/bin/bash

CONFIG_PATH=$1 #.env.integration

# if CONFIG_PATH is provided - source environment variables from it
if [ ! -z "$CONFIG_PATH" ]; then
    if [ ! -f "$CONFIG_PATH" ]; then
        echo "Configuration not found: $CONFIG_PATH"
        exit 1
    fi

    set -o allexport
    source .env.integration
    set +o allexport
fi

dotnet test --verbosity normal --filter TestCategory=Integration