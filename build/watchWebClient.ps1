$originalLocation = Get-Location
$projectRoot = "$PSScriptRoot/../"

try {
  Set-Location -Path $projectRoot
  . ./build/buildSettings.ps1

  Remove-Item -Path "$webProjectFolder/wwwroot/*" -Recurse
  Copy-Item -Path "$webClientProjectFolder/public/*" -Destination "$webProjectFolder/wwwroot" -Recurse

  Set-Location -Path $webClientProjectFolder

  npm install --no-audit
  npm run dev

} finally {
  Set-Location $originalLocation
}
