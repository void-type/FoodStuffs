. ./util.ps1

Push-Location -Path "$testProjectFolder"
dotnet watch test
Pop-Location
