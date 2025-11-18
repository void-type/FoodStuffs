[CmdletBinding()]
param (
  [Parameter(Mandatory = $false)]
  [string] $MigrationName,
  [Parameter(Mandatory = $false)]
  [string[]] $AdditionalArgs
)

$originalLocation = Get-Location
$projectRoot = "$PSScriptRoot/../"

try {
  Set-Location -Path $projectRoot
  . ./build/buildSettings.ps1

  $dbMigrationArgs = $dbMigrationArgs + $AdditionalArgs

  if ($MigrationName) {
    dotnet ef database update "$MigrationName" @dbMigrationArgs | Write-Output
    return
  }

  dotnet ef database update @dbMigrationArgs | Write-Output
} finally {
  Set-Location $originalLocation
}
