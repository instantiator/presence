#!/bin/bash

set -o allexport
source .env.integration
set +o allexport

dotnet test --verbosity normal --filter TestCategory=Integration "$@"