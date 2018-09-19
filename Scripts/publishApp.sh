#! /bin/bash

pushd "../FoodStuffs.Web/ClientApp"
yarn
yarn run vue-cli-service build --modern
popd
pushd "../FoodStuffs.Web/"
dotnet publish --configuration "Release" --output "out"
