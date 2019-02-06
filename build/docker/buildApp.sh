#! /bin/bash

# TODO: Find a way to build in docker with PowerShell https://github.com/dotnet/dotnet-docker/issues/360
pushd "../../src/FoodStuffs.Web/ClientApp"
npm run build
node tasks/set-version.js
popd

pushd "../../src/FoodStuffs.Web/"
dotnet publish --configuration "Release" --output "../../artifacts" --no-restore
popd
