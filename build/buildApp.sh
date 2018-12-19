#! /bin/bash

# TODO: Find a way to build in docker with PowerShell
pushd "../src/FoodStuffs.Web/ClientApp"
npm install
npm run build
node tasks/set-version.js
popd
pushd "../src/FoodStuffs.Web/"
dotnet publish --configuration "Release" --output "../../artifacts"
