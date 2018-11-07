. ./util.ps1

Push-Location -Path "$webProjectFolder"
dotnet build --configuration "Debug"
Pop-Location
