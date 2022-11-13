$originalLocation = Get-Location
$projectRoot = "$PSScriptRoot/../"

try {
  Set-Location -Path $projectRoot
  . ./build/buildSettings.ps1

  $contextDirectory = "../../$modelProjectFolder/Data/EntityFramework"
  $dataModelsFolder = "../../$modelProjectFolder/Data/Models/"
  $contextName = "$($shortAppName)Context"
  $settingsFile = "$webProjectFolder/appsettings.Development.json"

  if (-not (Test-Path -Path $settingsFile)) {
    throw "$settingsFile does not exist to get the Connection String from."
  }

  dotnet ef dbcontext scaffold Name=$shortAppName Microsoft.EntityFrameworkCore.SqlServer `
    --force `
    --startup-project "$webProjectFolder" `
    --context-dir "$contextDirectory" `
    --context "$contextName" `
    --namespace "FoodStuffs.Model.Data.Models" `
    --context-namespace "FoodStuffs.Model.Data.EntityFramework"`
    --output-dir "$dataModelsFolder"

  dotnet format

} finally {
  Set-Location $originalLocation
}
