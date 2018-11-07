# Run this script as a server administrator from the scripts directory

param(
  [switch] $Confirm
)

. ./util.ps1

$iisDirectory = "\\server1\c$\inetpub\wwwroot\$($shortAppName)"
$settingsDirectory = "\\server1\appSettings\$($shortAppName)"

if ($Confirm) {
  Push-Location -Path "../"
  New-Item -Path "$iisDirectory\app_offline.htm"
  ROBOCOPY "./artifacts" $iisDirectory /MIR
  Copy-Item -Path "$settingsDirectory\*" -Include "*.Production.json" -Recurse -Destination $iisDirectory
  Pop-Location
} else {
  Write-Error "Please use -Confirm for pushing to production."
}
