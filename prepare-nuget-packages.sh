#!/bin/bash

dotnet pack Presence.SocialFormat.Lib/Presence.SocialFormat.Lib.csproj --configuration Release --output nuget
dotnet pack Presence.Posting.Lib/Presence.Posting.Lib.csproj --configuration Release --output nuget