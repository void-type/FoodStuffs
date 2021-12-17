$shortAppName = "FoodStuffs"
$projectName = "$shortAppName"

$webProjectFolder = "./src/$projectName.Web"
$webClientProjectFolder = "$webProjectFolder/ClientApp"
$testProjectFolder = "./tests/$projectName.Test"

$iisDirectoryProduction = "\\server2\wwwroot\$($projectName)"
$settingsDirectoryProduction = "\\server2\Servers\webAppSettings\$($projectName)"

$iisDirectoryStaging = "\\server2\wwwroot\$($projectName)Test"
$settingsDirectoryStaging = "$settingsDirectoryProduction"
