Push-Location $PSScriptRoot

. ./util.ps1

Push-Location -Path "$webProjectFolder"
dotnet watch run --configuration "Debug" --launch-profile 'Kestrel (Development)'
Pop-Location

Pop-Location
