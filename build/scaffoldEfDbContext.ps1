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

if ($LASTEXITCODE -eq 0) {
  Write-Host "Within $dataModelsFolder, update namespaces."
  Write-Host "Within $contextName, add using for models namespace."
  Write-Host "Within $contextName, remove all ctors but the DbContextOptions one."
  Write-Host "Within $contextName, remove the OnConfiguring method as it contains sensitive information."
}

Pop-Location
Pop-Location
