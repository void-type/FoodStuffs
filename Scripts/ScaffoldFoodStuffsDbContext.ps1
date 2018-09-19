$projectName = "FoodStuffs"

Push-Location -Path "../$($projectName).Web"

$contextDirectory = "Data/EntityFramework"
$contextName = "$($projectName)Context"
$modelsDirectory = "../$($projectName).Model/Data/Models/"
$connectionString = Get-Content -Path "appsettings.Development.json" |
  ConvertFrom-Json |
  Select-Object -ExpandProperty ConnectionStrings |
  Select-Object -ExpandProperty $projectName

function Stop-OnError {
  if ($LASTEXITCODE -ne 0) {
    Pop-Location
    Exit $LASTEXITCODE
  }
}

dotnet ef dbcontext scaffold "$connectionString" Microsoft.EntityFrameworkCore.SqlServer --force --context-dir "$contextDirectory" --context "$contextName" --output-dir "$modelsDirectory"
Pop-Location
Stop-OnError

Write-Host "Within $modelsDirectory, update namespaces."
Write-Host "Within $contextName, add using for models namespace."
Write-Host "Within $contextName, update namespace. and add using for models namespace."
Write-Host "Within $contextName, remove the OnConfiguring method as it contains sensitive information."
Write-Host "Within $contextName, add a call to MapViews(modelBuilder); to OnModelCreating."

Pop-Location
