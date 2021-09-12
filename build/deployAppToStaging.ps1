# Run this script as a server administrator from the scripts directory
[CmdletBinding(SupportsShouldProcess = $true, ConfirmImpact = "Medium")]
param()

Push-Location -Path "$PSScriptRoot/../"
. ./build/util.ps1

$releaseFolder = './artifacts/dist/release'

try {
  if (-not (Test-Path -Path $releaseFolder)) {
    throw 'No artifacts to deploy. Run build.ps1 before deploying.'
  }

  if ($PSCmdlet.ShouldProcess("$iisDirectoryStaging", "Deploy $shortAppName to Staging.")) {
    New-Item -Path "$iisDirectoryStaging\app_offline.htm"
    Start-Sleep 5
    ROBOCOPY "$releaseFolder" "$iisDirectoryStaging" /MIR /XF "$iisDirectoryStaging\app_offline.htm"
    Copy-Item -Path "$settingsDirectoryStaging\*" -Include "*.Staging.json" -Recurse -Destination $iisDirectoryStaging
    Remove-Item -Path "$iisDirectoryStaging\app_offline.htm"
  }

} finally {
  Pop-Location
}
