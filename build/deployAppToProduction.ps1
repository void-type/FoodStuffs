# Run this script as a server administrator from the scripts directory

param(
  [switch] $Confirm
)

. ./util.ps1

if ($Confirm) {
  Push-Location -Path "../"
  New-Item -Path "$iisDirectoryProduction\app_offline.htm"
  ROBOCOPY "./artifacts" $iisDirectoryProduction /MIR
  Copy-Item -Path "$settingsDirectoryProduction\*" -Include "*.Production.json" -Recurse -Destination $iisDirectoryProduction
  Pop-Location
} else {
  Write-Error "Please use -Confirm for pushing to production."
}
