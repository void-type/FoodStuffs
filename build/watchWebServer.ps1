$originalLocation = Get-Location
$projectRoot = "$PSScriptRoot/../"

try {
  Set-Location -Path $projectRoot
  . ./build/buildSettings.ps1

  Set-Location $webProjectFolder

  dotnet watch

} finally {
  Set-Location $originalLocation
}
