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

  if (-not $DisableVueDevServer) {
    Remove-Item -Path "$webProjectFolder/wwwroot/*" -Recurse -ErrorAction SilentlyContinue
    Copy-Item -Path "$webClientProjectFolder/public/*" -Destination "$webProjectFolder/wwwroot" -Recurse

    Set-Location -Path $webClientProjectFolder
    npm install --no-audit
    npm run dev

  } else {
    Set-Location -Path $webClientProjectFolder
    npm install --no-audit
    npm run watch
  }


} finally {
  Set-Location $originalLocation
}
