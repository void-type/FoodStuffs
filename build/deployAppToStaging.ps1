# Run this script as a server administrator from the scripts directory
. ./util.ps1

Push-Location -Path "../"
New-Item -Path "$iisDirectoryStaging\app_offline.htm"
ROBOCOPY "./artifacts" $iisDirectoryStaging /MIR
Copy-Item -Path "$settingsDirectoryStaging\*" -Include "*.Staging.json" -Recurse -Destination $iisDirectoryStaging
Pop-Location
