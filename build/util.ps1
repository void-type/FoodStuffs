function Stop-OnError {
  if ($LASTEXITCODE -ne 0) {
    Pop-Location
    Exit $LASTEXITCODE
  }
}

$projectName = "FoodStuffs"

$dataModelsFolder = "../src/$projectName.Model/Data/Models/"
$testProjectFolder = "../tests/$projectName.Test"

$webProjectFolder = "../src/$projectName.Web"
$webClientProjectFolder = "$webProjectFolder/ClientApp"

$testProjectFolder = "../tests/$projectName.Test"

$iisDirectoryProduction = "\\server1\c$\inetpub\wwwroot\$($projectName)"
$settingsDirectoryProduction = "\\server1\appSettings\$($projectName)"

$iisDirectoryStaging = "\\server1\c$\inetpub\wwwroot\$($projectName)Test"
$settingsDirectoryStaging = "$settingsDirectoryProduction"
