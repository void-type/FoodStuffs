Push-Location -Path "../FoodStuffs.Web/ClientApp"
yarn
yarn run vue-cli-service build --mode "development" --modern
Pop-Location

Push-Location -Path "../FoodStuffs.Web"
dotnet build --configuration "Debug"
Pop-Location
