#! /bin/pwsh-preview

Push-Location -Path "../FoodStuffs.Web"
dotnet build --configuration Debug
Pop-Location
