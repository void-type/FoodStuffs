#! /bin/pwsh-preview

Push-Location -Path "../Core.Test"
dotnet watch test
Pop-Location
