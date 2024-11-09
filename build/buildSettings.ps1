$shortAppName = 'FoodStuffs'
$projectName = "$shortAppName"

$webClientProjectFolder = './src/ClientApp'
$testProjectFolder = "./tests/$projectName.Test"
$modelProjectFolder = "./src/$projectName.Model"
$webProjectFolder = "./src/$projectName.Web"
$modelProjectFolder = "./src/$projectName.Model"

$webReleaseFolder = './artifacts/dist/release/web'

$webDirectoryProduction = "\\server2\DeployedApps\apps\$projectName.Web"
$webSettingsDirectoryProduction = "\\server2\Servers\AppConfigs\$projectName\Web"

$webDirectoryTest = "\\server2\DeployedApps\apps\$($projectName).Web-Test"
$webSettingsDirectoryTest = "$webSettingsDirectoryProduction"

$dbMigrationArgs = @(
  '--project', (Resolve-Path "$PSScriptRoot/../$modelProjectFolder" | Select-Object -ExpandProperty Path),
  '--startup-project', (Resolve-Path "$PSScriptRoot/../$webProjectFolder" | Select-Object -ExpandProperty Path)
)
