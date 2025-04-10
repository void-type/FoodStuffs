$originalLocation = Get-Location
$projectRoot = "$PSScriptRoot/../"

try {
  Set-Location -Path "$projectRoot/src/ClientApp"

  npm install --no-audit
  # npm run generate-api-client

  # Fix because --disableStrictSSL doesn't seem to work, so we'll set the env var directly.
  # Issue noted: https://github.com/acacode/swagger-typescript-api/issues/669
  # Fix from different package: https://github.com/openapi-ts/openapi-typescript/issues/1512
  # Now set in the npm script
  # $env:NODE_TLS_REJECT_UNAUTHORIZED = 0

  npm run generate-api-client

} finally {
  Set-Location -Path $originalLocation
}
