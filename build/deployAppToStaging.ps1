# Run this script as a server administrator from the scripts directory

. ./util.ps1

if (-not (Test-Path -Path "../artifacts")) {
  Write-Error "No artifacts to deploy. Run build.ps1 before deploying."
  exit 1
}

Push-Location -Path "../"
New-Item -Path "$iisDirectoryStaging\app_offline.htm"
ROBOCOPY "./artifacts" $iisDirectoryStaging /MIR
Copy-Item -Path "$settingsDirectoryStaging\*" -Include "*.Staging.json" -Recurse -Destination $iisDirectoryStaging
Pop-Location
