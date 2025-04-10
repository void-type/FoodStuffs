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
    dotnet ef database update "$MigrationName" @dbMigrationArgs
    return
  }

  dotnet ef database update @dbMigrationArgs

} finally {
  Set-Location $originalLocation
}
