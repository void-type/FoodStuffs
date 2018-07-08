#! /bin/pwsh-preview

Push-Location -Path "../FoodStuffs.Web/ClientApp"
yarn
yarn run vue-cli-service build
Pop-Location
Set-Location -Path "../FoodStuffs.Web/"
dotnet publish --configuration Release --output out
Pop-Location
