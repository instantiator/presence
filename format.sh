#!/bin/bash

# pass parameters directly into the application
dotnet run --project Presence.SocialFormat.Console/Presence.SocialFormat.Console.csproj -- "$@"
