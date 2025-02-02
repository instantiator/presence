#!/bin/bash

# pass parameters directly into the application
dotnet run --project Presence.Posting.Console/Presence.Posting.Console.csproj -- "$@"
