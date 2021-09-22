Push-Location -Path "$PSScriptRoot/../"
. ./build/util.ps1
Pop-Location

$contextDirectory = "Data/EntityFramework"
$dataModelsFolder = "../$projectName.Model/Data/Models/"
$contextName = "$($shortAppName)Context"
$settingsFile = "appsettings.Development.json"

try {
  Push-Location -Path "$PSScriptRoot/../$webProjectFolder"

  if (-not (Test-Path -Path $settingsFile)) {
    throw "$settingsFile does not exist to get the Connection String from."
  }

  dotnet ef dbcontext scaffold Name=$shortAppName Microsoft.EntityFrameworkCore.SqlServer --force --context-dir "$contextDirectory" --context "$contextName" --output-dir "$dataModelsFolder"

} finally {
  Pop-Location
}
