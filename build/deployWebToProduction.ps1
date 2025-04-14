# Run this script as a server administrator from the scripts directory
[CmdletBinding(SupportsShouldProcess = $true, ConfirmImpact = 'High')]
param()

function Stop-OnError([string]$errorMessage) {
  if ($LASTEXITCODE -eq 0) {
    return
  }

  if (-not [string]::IsNullOrWhiteSpace($errorMessage)) {
    Write-Error $errorMessage
  }

  exit $LASTEXITCODE
}

$originalLocation = Get-Location
$projectRoot = "$PSScriptRoot/../"

try {
  Set-Location -Path $projectRoot
  . ./build/buildSettings.ps1

  if (-not (Test-Path -Path $webReleaseFolder)) {
    throw 'No artifacts to deploy. Run build.ps1 before deploying.'
  }

  if ($PSCmdlet.ShouldProcess("$webDirectoryProduction", "Deploy $shortAppName.Web to Production.")) {
    if (-not (Test-Path -Path $webSettingsProduction)) {
      throw "No settings file found at $webSettingsProduction"
    }

    New-Item -Path "$webDirectoryProduction\app_offline.htm" -Force

    $connectionString = Get-Content -Path $webSettingsProduction |
      ConvertFrom-Json |
      Select-Object -ExpandProperty ConnectionStrings |
      Select-Object -ExpandProperty $shortAppName

    $censoredConnectionString = $connectionString -replace '(?<=Password=)(.*?)(?=;)', '***'

    Write-Host "Running database migration with connection string:`n$censoredConnectionString"

    $migrationArgs = @(
      '--connection', $connectionString
    )

    .$PSScriptRoot/dbApplyMigration.ps1 -AdditionalArgs $migrationArgs
    Stop-OnError -errorMessage 'Database migration failed.'

    Start-Sleep 5

    ROBOCOPY "$webReleaseFolder" "$webDirectoryProduction" /MIR /XF "$webDirectoryProduction\app_offline.htm"
    Copy-Item -Path "$webSettingsProduction" -Destination $webDirectoryProduction
    Remove-Item -Path "$webDirectoryProduction\app_offline.htm"
  }

} finally {
  Set-Location $originalLocation
}
