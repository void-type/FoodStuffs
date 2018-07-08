#! /bin/pwsh-preview

Push-Location -Path "../FoodStuffs.Web/ClientApp"
yarn run vue-cli-service test:unit
Pop-Location
