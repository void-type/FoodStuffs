function Stop-OnError {
  if ($LASTEXITCODE -ne 0) {
    Pop-Location
    Exit $LASTEXITCODE
  }
}

$shortAppName = "FoodStuffs"
$projectName = "$shortAppName"

$dataModelsFolder = "../src/$projectName.Model/Data/Models/"
$testProjectFolder = "../tests/$projectName.Test"

$webProjectFolder = "../src/$projectName.Web"
$webClientProjectFolder = "$webProjectFolder/ClientApp"

$testProjectFolder = "../tests/$projectName.Test"

$iisDirectoryProduction = "\\server2\c$\inetpub\wwwroot\$($projectName)"
$settingsDirectoryProduction = "\\server2\appSettings\$($projectName)"

$iisDirectoryStaging = "\\server2\c$\inetpub\wwwroot\$($projectName)Test"
$settingsDirectoryStaging = "$settingsDirectoryProduction"
