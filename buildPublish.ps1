#! /usr/bin/pwsh

# Build the entire application to *.Web/out
dotnet restore
Set-Location FoodStuffs.Web/ClientApp
yarn
yarn build
Set-Location ../../
dotnet publish FoodStuffs.Web -c Release -o out