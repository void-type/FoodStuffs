$shortAppName = "FoodStuffs"
$projectName = "$shortAppName"

$webProjectFolder = "./src/$projectName.Web"
$webClientProjectFolder = "./src/ClientApp"
$testProjectFolder = "./tests/$projectName.Test"

$iisDirectoryProduction = "\\server2\DeployedApps\apps\$projectName"
$settingsDirectoryProduction = "\\server2\Servers\AppConfigs\$projectName"

$iisDirectoryTest = "\\server2\DeployedApps\apps\$($projectName)-Test"
$settingsDirectoryTest = "$settingsDirectoryProduction"
