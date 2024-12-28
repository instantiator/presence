#!/bin/bash

PACKAGE_NAME=Presence.SocialFormat.Lib

dotnet pack ${PACKAGE_NAME}/${PACKAGE_NAME}.csproj --configuration Release --output nuget