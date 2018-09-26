#! /bin/bash

# TODO: Find a way to build in docker with powershell
pushd "../FoodStuffs.Web/ClientApp"
yarn
yarn run vue-cli-service build
popd
pushd "../FoodStuffs.Web/"
dotnet publish --configuration "Release" --output "out"
