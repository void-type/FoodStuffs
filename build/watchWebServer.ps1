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

  # Work around dotnet watch crashing on NBGV's shared PrivateP2PCaching.proj (dotnet/sdk#54552).
  $env:NBGV_CacheMode = 'None'

  dotnet watch -- /DisableVueDevServer=$DisableVueDevServer

} finally {
  Set-Location $originalLocation
}
