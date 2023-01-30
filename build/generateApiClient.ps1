$originalLocation = Get-Location
$projectRoot = "$PSScriptRoot/../"

try {
  Set-Location -Path "$projectRoot/src/ClientApp"

  npm install --no-audit
  npm run generate-api-client

} finally {
  Set-Location -Path $originalLocation
}
