# Run this script as a server administrator from the scripts directory

param(
  [switch] $Confirm
)

. ./util.ps1

if (-not $Confirm) {
  Write-Error "Please use -Confirm for pushing to production."
  exit 1
}

if (-not (Test-Path -Path "../artifacts")) {
  Write-Error "No artifacts to deploy. Run build.ps1 before deploying."
  exit 1
}

Push-Location -Path "../"
New-Item -Path "$iisDirectoryProduction\app_offline.htm"
ROBOCOPY "./artifacts" $iisDirectoryProduction /MIR
Copy-Item -Path "$settingsDirectoryProduction\*" -Include "*.Production.json" -Recurse -Destination $iisDirectoryProduction
Pop-Location
