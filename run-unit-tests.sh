#!/bin/bash

dotnet test --verbosity normal --filter TestCategory=Unit "$@"