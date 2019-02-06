[CmdletBinding()]
param(
  [string] $Configuration = "Release",
  [switch] $SkipClient,
  [switch] $SkipPublish,
  [switch] $SkipTest
)

. ./util.ps1

$nodeModes = @{
  "Release" = "production";
  "Debug"   = "development"
}

# Clean the artifacts folders
Remove-Item -Path "../artifacts" -Recurse -ErrorAction SilentlyContinue
Remove-Item -Path "../coverage" -Recurse -ErrorAction SilentlyContinue
Remove-Item -Path "../testResults" -Recurse -ErrorAction SilentlyContinue

if (-not $SkipClient) {
  # Lint, test and build client
  Push-Location -Path "$webClientProjectFolder"
  npm install
  npm run lint
  Stop-OnError

  if (-not $SkipTest) {
    npm run test:unit
    Stop-OnError
  }

  npm run build -- --mode "$($nodeModes[$Configuration])"
  Stop-OnError
  node "tasks/set-version.js"
  Stop-OnError
  Pop-Location
}

# Build solution
Push-Location -Path "../"
dotnet build --configuration "$Configuration"
Stop-OnError
Pop-Location

if (-not $SkipTest) {
  # Run tests, gather coverage
  Push-Location -Path "$testProjectFolder"

  dotnet test `
    --configuration "$Configuration" `
    --no-build `
    --logger 'trx' `
    --results-directory '../../testResults' `
    /p:Exclude='[xunit.runner.*]*' `
    /p:CollectCoverage=true `
    /p:CoverletOutputFormat=cobertura `
    /p:CoverletOutput="../../coverage/coverage.cobertura.xml"

  Stop-OnError
  Pop-Location

  # Generate code coverage report
  Push-Location -Path "../coverage"
  reportgenerator "-reports:coverage.cobertura.xml" "-targetdir:." "-reporttypes:HtmlInline_AzurePipelines"
  Stop-OnError
  Pop-Location
}

if (-not $SkipPublish) {
  # Package build
  Push-Location -Path "$webProjectFolder"
  dotnet publish --configuration "$Configuration" --no-build --output "../../artifacts"
  Stop-OnError
  Pop-Location
}
