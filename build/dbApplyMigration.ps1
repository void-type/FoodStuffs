[CmdletBinding()]
param (
  [Parameter(Mandatory = $false)]
  [string] $MigrationName,
  [Parameter(Mandatory = $false)]
  [string[]] $AdditionalArgs,
  # Use the already-built startup project output instead of rebuilding.
  # Useful when the app is running locally and a rebuild would fail due to locked files.
  [switch] $NoBuild,
  # Build configuration to use (dotnet ef defaults to Debug if not specified).
  # Deploy scripts pass 'Release' so the migration matches the build being deployed.
  [string] $Configuration
)

$originalLocation = Get-Location
$projectRoot = "$PSScriptRoot/../"

try {
  Set-Location -Path $projectRoot
  . ./build/buildSettings.ps1

  $dbMigrationArgs = $dbMigrationArgs + $AdditionalArgs

  if ($NoBuild) {
    $dbMigrationArgs = $dbMigrationArgs + '--no-build'
  }

  if ($Configuration) {
    $dbMigrationArgs = $dbMigrationArgs + @('--configuration', $Configuration)
  }

  if ($MigrationName) {
    dotnet ef database update "$MigrationName" @dbMigrationArgs | Write-Output
    return
  }

  dotnet ef database update @dbMigrationArgs | Write-Output
} finally {
  Set-Location $originalLocation
}
