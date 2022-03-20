$shortAppName = "FoodStuffs"
$projectName = "$shortAppName"

$webProjectFolder = "./src/$projectName.Web"
$webClientProjectFolder = "$webProjectFolder/ClientApp"
$testProjectFolder = "./tests/$projectName.Test"

$iisDirectoryProduction = "\\server2\DeployedApps\apps\$($projectName)"
$settingsDirectoryProduction = "\\server2\Servers\AppConfigs\$($projectName)"

$iisDirectoryStaging = "\\server2\DeployedApps\apps\$($projectName)Test"
$settingsDirectoryStaging = "$settingsDirectoryProduction"
