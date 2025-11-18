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
    dotnet ef migrations remove "$MigrationName" @dbMigrationArgs | Write-Output
    return
  }

  dotnet ef migrations remove @dbMigrationArgs | Write-Output

} finally {
  Set-Location $originalLocation
}
