. ./util.ps1

dotnet tool install --global dotnet-ef
dotnet tool update --global dotnet-ef

Push-Location -Path "$webProjectFolder"

$contextDirectory = "Data/EntityFramework"
$contextName = "$($shortAppName)Context"
$settingsFile = "appsettings.Development.json"

if (-not (Test-Path -Path $settingsFile)) {
  Write-Host "$settingsFile does not exist to get the Connection String from."
}

$connectionString = Get-Content -Path "$settingsFile" |
  ConvertFrom-Json |
  Select-Object -ExpandProperty ConnectionStrings |
  Select-Object -ExpandProperty $shortAppName

dotnet ef dbcontext scaffold "$connectionString" Microsoft.EntityFrameworkCore.SqlServer --force --context-dir "$contextDirectory" --context "$contextName" --output-dir "../$dataModelsFolder"

Write-Host "Within $dataModelsFolder, update namespaces."
Write-Host "Within $contextName, add using for models namespace."
Write-Host "Within $contextName, remove all ctors but the DbContextOptions one."
Write-Host "Within $contextName, remove the OnConfiguring method as it contains sensitive information."

Pop-Location
