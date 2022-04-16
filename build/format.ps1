[CmdletBinding()]
param(
  [ValidateSet("All", "Server", "Client")]
  [string] $Target = 'All'
)

$originalLocation = Get-Location
$projectRoot = "$PSScriptRoot/../"

try {
  Set-Location -Path $projectRoot
  . ./build/buildSettings.ps1

  if ($Target -ne 'Client') {
    dotnet format
  }

  if ($Target -ne 'Server') {
    Set-Location -Path $webClientProjectFolder
    npm run lint
    npm run format
  }
} finally {
  Set-Location $originalLocation
}
