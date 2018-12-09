#! /bin/bash

# TODO: Find a way to build in docker with powershell
pushd "../src/FoodStuffs.Web/ClientApp"
npm install
npm run build
popd
pushd "../src/FoodStuffs.Web/"
dotnet publish --configuration "Release" --output "../../artifacts"
