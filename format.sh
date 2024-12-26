#!/bin/bash

# pass parameters directly into the application
dotnet run --project SocialFormat.Console/SocialFormat.Console.csproj -- "$@"
