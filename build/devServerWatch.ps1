. ./util.ps1

Push-Location -Path "$webProjectFolder"
dotnet watch run --configuration "Release" --launch-profile 'Kestrel (Development)'
Pop-Location
