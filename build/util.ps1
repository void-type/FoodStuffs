function Stop-OnError {
  if ($LASTEXITCODE -ne 0) {
    Pop-Location
    Exit $LASTEXITCODE
  }
}

$shortAppName = "FoodStuffs"
$projectName = "$($shortAppName)"

$dataModelsFolder = "../src/$projectName.Model/Data/Models/"
$testProjectFolder = "../tests/$projectName.Test"

$webProjectFolder = "../src/$projectName.Web"
$webClientProjectFolder = "$webProjectFolder/ClientApp"

$iisDirectoryProduction = "\\server1\c$\inetpub\wwwroot\$($shortAppName)"
$settingsDirectoryProduction = "\\server1\appSettings\$($shortAppName)"

$iisDirectoryStaging = "\\server1\c$\inetpub\wwwroot\$($shortAppName)Test"
$settingsDirectoryStaging = "$settingsDirectoryProduction"
