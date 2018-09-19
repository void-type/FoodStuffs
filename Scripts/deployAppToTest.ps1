# Run this script as a server administrator from the scripts directory

$serverName = "Server1"

$iisDirectory = "\\$serverName\c$\inetpub\wwwroot\FoodStuffsTest"
$settingsDirectory = "\\$serverName\appSettings\FoodStuffsSettings"

Push-Location -Path "../FoodStuffs.Web"
New-Item -Path "$iisDirectory\app_offline.htm"
ROBOCOPY "out" $iisDirectory /MIR
Copy-Item -Path "$settingsDirectory\*" -Include "*.Staging.json" -Recurse -Destination $iisDirectory
Pop-Location
