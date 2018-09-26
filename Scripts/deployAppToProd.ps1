# Run this script as a server administrator from the scripts directory

param(
  [switch] $Confirm
)

Write-Error "You are not ready to wield this power. Get sign-off first."
exit

$serverName = "Server1"

$iisDirectory = "\\$serverName\c$\inetpub\wwwroot\FoodStuffsTest"
$settingsDirectory = "\\$serverName\appSettings\FoodStuffsSettings"

if ($Confirm) {
  Push-Location -Path "../FoodStuffs.Web"
  New-Item -Path "$iisDirectory\app_offline.htm"
  ROBOCOPY "out" $iisDirectory /MIR
  Copy-Item -Path "$settingsDirectory\*" -Include "*.Production.json" -Recurse -Destination $iisDirectory
  Pop-Location
}
else {
  Write-Error "Please use -Confirm for pushing to production."
}

