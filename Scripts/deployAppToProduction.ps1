# Run this script as a server administrator from the scripts directory

param(
  [switch] $Confirm
)

. ./util.ps1

$iisDirectory = "\\server1\c$\inetpub\wwwroot\$($shortAppName)Test"
$settingsDirectory = "\\server1\appSettings\$($shortAppName)Test"

if ($Confirm) {
  Push-Location -Path "$webProjectFolder"
  New-Item -Path "$iisDirectory\app_offline.htm"
  ROBOCOPY "out" $iisDirectory /MIR
  Copy-Item -Path "$settingsDirectory\*" -Include "*.Production.json" -Recurse -Destination $iisDirectory
  Pop-Location
} else {
  Write-Error "Please use -Confirm for pushing to production."
}

