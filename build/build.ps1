[CmdletBinding()]
param(
  [string] $Configuration = "Release",
  [switch] $SkipFormat,
  [switch] $SkipOutdated,
  [switch] $SkipClient,
  [switch] $SkipTest,
  [switch] $SkipTestReport,
  [switch] $SkipPublish
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

# Lint, test and build client
if (-not $SkipClient) {
  Push-Location -Path "$webClientProjectFolder"
  npm install

  if (-not $SkipFormat) {
    npm run lint
    Stop-OnError
  }

  if (-not $SkipOutdated) {
    npm outdated
  }

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

# Restore local dotnet tools
dotnet tool restore

if (-not $SkipFormat) {
  dotnet format --check
  Stop-OnError
}

dotnet restore

if (-not $SkipOutdated) {
  dotnet outdated
}

dotnet build --configuration "$Configuration" --no-restore
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
    /p:Exclude="[xunit.*]*%2c[$projectName.Test]*%2c[*]ThisAssembly" `
    /p:CollectCoverage=true `
    /p:CoverletOutputFormat=cobertura `
    /p:CoverletOutput="../../coverage/coverage.cobertura.xml"

  Stop-OnError
  Pop-Location

  if (-not $SkipTestReport) {
    # Generate code coverage report
    Push-Location -Path "../coverage"
    dotnet reportgenerator "-reports:coverage.cobertura*.xml" "-targetdir:." "-reporttypes:HtmlInline_AzurePipelines"
    Stop-OnError
    Pop-Location
  }
}

if (-not $SkipPublish) {
  # Package build
  Push-Location -Path "$webProjectFolder"
  dotnet publish --configuration "$Configuration" --no-build --output "../../artifacts"
  Stop-OnError
  Pop-Location
}
