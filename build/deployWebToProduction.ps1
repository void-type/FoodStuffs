# Run this script as a server administrator from the scripts directory
[CmdletBinding(SupportsShouldProcess = $true, ConfirmImpact = "High")]
param()

$originalLocation = Get-Location
$projectRoot = "$PSScriptRoot/../"

try {
  Set-Location -Path $projectRoot
  . ./build/buildSettings.ps1

  if (-not (Test-Path -Path $webReleaseFolder)) {
    throw 'No artifacts to deploy. Run build.ps1 before deploying.'
  }

  if ($PSCmdlet.ShouldProcess("$webDirectoryProduction", "Deploy $shortAppName.Web to Production.")) {
    New-Item -Path "$webDirectoryProduction\app_offline.htm" -Force
    Start-Sleep 5
    ROBOCOPY "$webReleaseFolder" "$webDirectoryProduction" /MIR /XF "$webDirectoryProduction\app_offline.htm"
    Copy-Item -Path "$webSettingsDirectoryProduction\*" -Include "*.Production.json" -Recurse -Destination $webDirectoryProduction
    Remove-Item -Path "$webDirectoryProduction\app_offline.htm"
  }

} finally {
  Set-Location $originalLocation
}
