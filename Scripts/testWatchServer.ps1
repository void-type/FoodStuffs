#! /bin/pwsh-preview

Push-Location -Path "../FoodStuffs.Test"
dotnet watch test
Pop-Location
