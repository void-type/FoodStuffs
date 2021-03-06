# Run this script as a server administrator from the scripts directory
[CmdletBinding(SupportsShouldProcess = $true, ConfirmImpact = "Medium")]
param()

Push-Location $PSScriptRoot

. ./util.ps1

if (-not (Test-Path -Path "../artifacts")) {
  Write-Error "No artifacts to deploy. Run build.ps1 before deploying."
  exit 1
}

Push-Location -Path "../"

try {
  if ($PSCmdlet.ShouldProcess("$iisDirectoryStaging", "Deploy $shortAppName to Staging.")) {
    New-Item -Path "$iisDirectoryStaging\app_offline.htm"
    Start-Sleep 5
    ROBOCOPY "./artifacts" $iisDirectoryStaging /MIR /XF "$iisDirectoryStaging\app_offline.htm"
    Copy-Item -Path "$settingsDirectoryStaging\*" -Include "*.Staging.json" -Recurse -Destination $iisDirectoryStaging
    Remove-Item -Path "$iisDirectoryStaging\app_offline.htm"
  }
} finally {
  Pop-Location
  Pop-Location
}
