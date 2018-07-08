#! /bin/pwsh-preview

Push-Location -Path "../Core.Test"
dotnet test /p:CollectCoverage=true
Pop-Location
