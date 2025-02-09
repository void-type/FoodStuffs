[CmdletBinding()]
param (
  [Parameter()]
  [switch]
  $DisableVueDevServer
)

$originalLocation = Get-Location
$projectRoot = "$PSScriptRoot/../"

try {
  Set-Location -Path $projectRoot
  . ./build/buildSettings.ps1

  Set-Location $webProjectFolder

  $env:DOTNET_WATCH_RESTART_ON_RUDE_EDIT = 'true'

  dotnet watch -- /DisableVueDevServer=$DisableVueDevServer

} finally {
  Set-Location $originalLocation
}
