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

  if ($MigrationName) {
    dotnet ef migrations remove "$MigrationName" @dbMigrationArgs
    return
  }

  dotnet ef migrations remove @dbMigrationArgs

} finally {
  Set-Location $originalLocation
}
