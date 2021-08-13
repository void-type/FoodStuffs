function Stop-OnError {
  if ($LASTEXITCODE -ne 0) {
    Pop-Location
    Exit $LASTEXITCODE
  }
}

$shortAppName = "FoodStuffs"
$projectName = "$shortAppName"
$projectVersion = (dotnet nbgv get-version -f json | ConvertFrom-Json).NuGetPackageVersion

$dataModelsFolder = "../src/$projectName.Model/Data/Models/"
$testProjectFolder = "../tests/$projectName.Test"

$webProjectFolder = "../src/$projectName.Web"
$webClientProjectFolder = "$webProjectFolder/ClientApp"

$iisDirectoryProduction = "\\server2\wwwroot\$($projectName)"
$settingsDirectoryProduction = "\\server2\Servers\webAppSettings\$($projectName)"

$iisDirectoryStaging = "\\server2\wwwroot\$($projectName)Test"
$settingsDirectoryStaging = "$settingsDirectoryProduction"
