$originalLocation = Get-Location
$projectRoot = "$PSScriptRoot/../"

try {
  Set-Location -Path $projectRoot
  . ./build/util.ps1

  Set-Location -Path $webClientProjectFolder

  npm install
  npm run build -- --watch --mode "development"

} finally {
  Set-Location $originalLocation
}
