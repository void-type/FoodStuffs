# Run this script as a server administrator from the scripts directory
[CmdletBinding(SupportsShouldProcess = $true, ConfirmImpact = "Medium")]
param()

$originalLocation = Get-Location
$projectRoot = "$PSScriptRoot/../"

try {
  Set-Location -Path $projectRoot
  . ./build/buildSettings.ps1

  if (-not (Test-Path -Path $webReleaseFolder)) {
    throw 'No artifacts to deploy. Run build.ps1 before deploying.'
  }

  if ($PSCmdlet.ShouldProcess("$webDirectoryTest", "Deploy $shortAppName.Web to Test.")) {
    New-Item -Path "$webDirectoryTest\app_offline.htm" -Force
    Start-Sleep 5
    ROBOCOPY "$webReleaseFolder" "$webDirectoryTest" /MIR /XF "$webDirectoryTest\app_offline.htm"
    Copy-Item -Path "$webSettingsDirectoryTest\*" -Include "*.Test.json" -Recurse -Destination $webDirectoryTest
    Remove-Item -Path "$webDirectoryTest\app_offline.htm"
  }

} finally {
  Set-Location $originalLocation
}
