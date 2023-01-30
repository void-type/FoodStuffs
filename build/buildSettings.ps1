$shortAppName = "FoodStuffs"
$projectName = "$shortAppName"

$webProjectFolder = "./src/$projectName.Web"
$modelProjectFolder = "./src/$projectName.Model"
$webClientProjectFolder = "./src/ClientApp"
$iisDirectoryProduction = "\\server2\DeployedApps\apps\$projectName"
$settingsDirectoryProduction = "\\server2\Servers\AppConfigs\$projectName"

$testProjectFolder = "./tests/$projectName.Test"

$iisDirectoryTest = "\\server2\DeployedApps\apps\$($projectName)-Test"
$settingsDirectoryTest = "$settingsDirectoryProduction"
