# Run this script as a server administrator from the scripts directory
[CmdletBinding(SupportsShouldProcess = $true, ConfirmImpact = "High")]
param()

$originalLocation = Get-Location
$projectRoot = "$PSScriptRoot/../"

try {
  Set-Location -Path $projectRoot
  . ./build/util.ps1

  $releaseFolder = './artifacts/dist/release'

  if (-not (Test-Path -Path $releaseFolder)) {
    throw 'No artifacts to deploy. Run build.ps1 before deploying.'
  }

  if ($PSCmdlet.ShouldProcess("$iisDirectoryProduction", "Deploy $shortAppName to Production.")) {
    New-Item -Path "$iisDirectoryProduction\app_offline.htm"
    Start-Sleep 5
    ROBOCOPY "$releaseFolder" "$iisDirectoryProduction" /MIR /XF "$iisDirectoryProduction\app_offline.htm"
    Copy-Item -Path "$settingsDirectoryProduction\*" -Include "*.Production.json" -Recurse -Destination $iisDirectoryProduction
    Remove-Item -Path "$iisDirectoryProduction\app_offline.htm"
  }

} finally {
  Set-Location $originalLocation
}
