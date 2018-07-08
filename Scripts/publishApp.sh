#! /bin/bash

pushd "../FoodStuffs.Web/ClientApp"
yarn
yarn run vue-cli-service build
popd
pushd "../FoodStuffs.Web/"
dotnet publish --configuration Release --output out
