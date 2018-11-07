# Run this script as a server administrator from the scripts directory
. ./util.ps1

$iisDirectory = "\\server1\c$\inetpub\wwwroot\$($shortAppName)Test"
$settingsDirectory = "\\server1\appSettings\$($shortAppName)"

Push-Location -Path "../"
New-Item -Path "$iisDirectory\app_offline.htm"
ROBOCOPY "./artifacts" $iisDirectory /MIR
Copy-Item -Path "$settingsDirectory\*" -Include "*.Staging.json" -Recurse -Destination $iisDirectory
Pop-Location
