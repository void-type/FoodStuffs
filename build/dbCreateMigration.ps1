[CmdletBinding()]
param (
  [Parameter(Mandatory = $true)]
  [string] $MigrationName
)

$originalLocation = Get-Location
$projectRoot = "$PSScriptRoot/../"

try {
  Set-Location -Path $projectRoot
  . ./build/buildSettings.ps1

  dotnet ef migrations add "$MigrationName" @dbMigrationArgs

} finally {
  Set-Location $originalLocation
}
