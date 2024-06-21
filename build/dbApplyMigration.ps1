[CmdletBinding()]
param (
  [Parameter(Mandatory = $false)]
  [string] $MigrationName
)

$originalLocation = Get-Location
$projectRoot = "$PSScriptRoot/../"

try {
  Set-Location -Path $projectRoot
  . ./build/buildSettings.ps1

  $argus = @(
    'ef'
    'database'
    'update'
    '--project'
    $modelProjectFolder
    '--startup-project'
    $webProjectFolder
  )

  if (-not [string]::IsNullOrWhiteSpace($MigrationName)) {
    $argus += $MigrationName
  }

  & dotnet $argus


} finally {
  Set-Location $originalLocation
}
