function Stop-OnError {
  if ($LASTEXITCODE -ne 0) {
    Pop-Location
    Exit $LASTEXITCODE
  }
}

$shortAppName = "FoodStuffs"
$projectName = "$($shortAppName)"
$dataModelsFolder = "../$projectName.Model/Data/Models/"
$testProjectFolder = "../$projectName.Test"

$webProjectFolder = "../$projectName.Web"
$webClientProjectFolder = "$webProjectFolder/ClientApp";
