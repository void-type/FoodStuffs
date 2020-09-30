# Run this script as a server administrator from the scripts directory
[CmdletBinding(SupportsShouldProcess = $true, ConfirmImpact = "High")]
param()

Push-Location $PSScriptRoot

. ./util.ps1

if (-not (Test-Path -Path "../artifacts")) {
  Write-Error "No artifacts to deploy. Run build.ps1 before deploying."
  exit 1
}

Push-Location -Path "../"

try {
  if ($PSCmdlet.ShouldProcess("$iisDirectoryProduction", "Deploy $shortAppName to Production.")) {
    New-Item -Path "$iisDirectoryProduction\app_offline.htm"
    Start-Sleep 5
    ROBOCOPY "./artifacts" $iisDirectoryProduction /MIR /XF "$iisDirectoryProduction\app_offline.htm"
    Copy-Item -Path "$settingsDirectoryProduction\*" -Include "*.Production.json" -Recurse -Destination $iisDirectoryProduction
    Remove-Item -Path "$iisDirectoryProduction\app_offline.htm"
  }
} finally {
  Pop-Location
  Pop-Location
}
