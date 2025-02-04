#!/bin/bash

mkdir -p release
rm -r release/*
for APP_NAME in Presence.SocialFormat.Console Presence.Posting.Console; do
    for OS in win linux osx; do
        # not self contained
        mkdir -p "release/${OS}-x64"
        dotnet publish ${APP_NAME}/${APP_NAME}.csproj \
            -c Release \
            --os $OS \
            /p:PublishSingleFile=true \
            /p:CopyOutputSymbolsToPublishDirectory=true \
            --self-contained false \
            --output "release/${OS}-x64"
        chmod +x release/${OS}-x64/${APP_NAME}*

        # self contained
        mkdir -p "release/${OS}-x64/self-contained"
        dotnet publish ${APP_NAME}/${APP_NAME}.csproj \
            -c Release \
            --os $OS \
            /p:PublishSingleFile=true \
            /p:CopyOutputSymbolsToPublishDirectory=true \
            --self-contained true \
            --output "release/${OS}-x64/self-contained"
        chmod +x release/${OS}-x64/self-contained/${APP_NAME}*
    done
done