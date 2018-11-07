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
$webClientProjectFolder = "$webProjectFolder/ClientApp";
