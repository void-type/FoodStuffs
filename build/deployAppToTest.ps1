# Run this script as a server administrator from the scripts directory
[CmdletBinding(SupportsShouldProcess = $true, ConfirmImpact = "Medium")]
param()

$originalLocation = Get-Location
$projectRoot = "$PSScriptRoot/../"

try {
  Set-Location -Path $projectRoot
  . ./build/buildSettings.ps1

  $releaseFolder = './artifacts/dist/release'

  if (-not (Test-Path -Path $releaseFolder)) {
    throw 'No artifacts to deploy. Run build.ps1 before deploying.'
  }

  if ($PSCmdlet.ShouldProcess("$iisDirectoryTest", "Deploy $shortAppName to Test.")) {
    New-Item -Path "$iisDirectoryTest\app_offline.htm" -Force
    Start-Sleep 5
    ROBOCOPY "$releaseFolder" "$iisDirectoryTest" /MIR /XF "$iisDirectoryTest\app_offline.htm"
    Copy-Item -Path "$settingsDirectoryTest\*" -Include "*.Test.json" -Recurse -Destination $iisDirectoryTest
    Remove-Item -Path "$iisDirectoryTest\app_offline.htm"
  }

} finally {
  Set-Location $originalLocation
}
