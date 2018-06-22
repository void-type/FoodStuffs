#! /bin/bash

cd ../FoodStuffs.Web/ClientApp
yarn
yarn run vue-cli-service build
cd ../
dotnet publish --configuration Release --output out