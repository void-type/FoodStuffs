#! /bin/bash

# TODO: Find a way to build in docker with PowerShell
# TODO: docker build cannot restore VoidCore unless local feed is configured in the container.
pushd "../../src/FoodStuffs.Web/ClientApp"
npm run build
node tasks/set-version.js
popd

pushd "../../src/FoodStuffs.Web/"
dotnet publish --configuration "Release" --output "../../artifacts" --no-restore
popd
