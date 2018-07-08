#! /bin/pwsh-preview

Push-Location -Path "../FoodStuffs.Test"
dotnet test /p:CollectCoverage=true
Pop-Location
