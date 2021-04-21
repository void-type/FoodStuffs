Push-Location $PSScriptRoot

. ./util.ps1

Push-Location -Path "$webProjectFolder"

$contextDirectory = "Data/EntityFramework"
$contextName = "$($shortAppName)Context"
$settingsFile = "appsettings.Development.json"

if (-not (Test-Path -Path $settingsFile)) {
  Write-Host "$settingsFile does not exist to get the Connection String from."
}

dotnet ef dbcontext scaffold Name=$shortAppName Microsoft.EntityFrameworkCore.SqlServer --force --context-dir "$contextDirectory" --context "$contextName" --output-dir "../$dataModelsFolder"

Pop-Location
Pop-Location
