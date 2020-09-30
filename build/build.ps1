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

$nodeModes = @{
  "Release" = "production";
  "Debug"   = "development"
}

Push-Location $PSScriptRoot

# Clean the artifacts folders
Remove-Item -Path "../artifacts" -Recurse -ErrorAction SilentlyContinue
Remove-Item -Path "../coverage" -Recurse -ErrorAction SilentlyContinue
Remove-Item -Path "../testResults" -Recurse -ErrorAction SilentlyContinue

# Restore local dotnet tools
Push-Location -Path "../"
dotnet tool restore
Pop-Location

. ./util.ps1

# Lint and build client
if (-not $SkipClient) {
  Push-Location -Path "$webClientProjectFolder"
  npm install

  if (-not $SkipFormat) {
    npm run lint
    Stop-OnError
  }

  if (-not $SkipOutdated) {
    npm audit --production
  }

  npm run build -- --mode "$($nodeModes[$Configuration])"
  Stop-OnError
  Pop-Location
}

# Build solution
Push-Location -Path "../"

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

Pop-Location

Write-Host "`nBuilt $projectName $projectVersion`n"
