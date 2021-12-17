$originalLocation = Get-Location
$projectRoot = "$PSScriptRoot/../"

try {
  Set-Location -Path $projectRoot
  . ./build/util.ps1

  $contextDirectory = "Data/EntityFramework"
  $dataModelsFolder = "../$projectName.Model/Data/Models/"
  $contextName = "$($shortAppName)Context"
  $settingsFile = "appsettings.Development.json"

  Set-Location -Path $webProjectFolder

  if (-not (Test-Path -Path $settingsFile)) {
    throw "$settingsFile does not exist to get the Connection String from."
  }

  dotnet ef dbcontext scaffold Name=$shortAppName Microsoft.EntityFrameworkCore.SqlServer --force --context-dir "$contextDirectory" --context "$contextName" --output-dir "$dataModelsFolder"

} finally {
  Set-Location $originalLocation
}
