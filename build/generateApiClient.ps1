$originalLocation = Get-Location
$projectRoot = "$PSScriptRoot/../"

try {
  Set-Location -Path "$projectRoot/src/ClientApp"

  npm install --no-audit
  # npm run generate-api-client

  # Fix because --disableStrictSSL doesn't seem to work, so we'll set the env var directly.
  # Issue noted: https://github.com/acacode/swagger-typescript-api/issues/669
  # Fix from different package: https://github.com/openapi-ts/openapi-typescript/issues/1512
  $env:NODE_TLS_REJECT_UNAUTHORIZED = 0
  npx sta -p https://localhost:5001/swagger/v1/swagger.json -o ./src/api/ --modular --responses --extract-request-params --disableStrictSSL

} finally {
  Set-Location -Path $originalLocation
}
