[CmdletBinding()]
param(
  [string] $Configuration = 'Release',
  [switch] $SkipFormat,
  [switch] $SkipOutdated,
  [switch] $SkipClient,
  [switch] $SkipTest,
  [switch] $SkipTestReport,
  [switch] $SkipPublish
)

$nodeModes = @{
  'Release' = 'production'
  'Debug'   = 'development'
}

Push-Location -Path "$PSScriptRoot/../"
. ./build/util.ps1

try {
  # Clean the artifacts folders
  Remove-Item -Path './artifacts' -Recurse -ErrorAction SilentlyContinue

  # Restore local dotnet tools
  dotnet tool restore

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
  if (-not $SkipFormat) {
    dotnet format --check --fix-whitespace --fix-style warn
    if ($LASTEXITCODE -ne 0) {
      Write-Error 'Please run formatter: dotnet format --fix-whitespace --fix-style warn.'
    }
    Stop-OnError
  }

  dotnet restore

  if (-not $SkipOutdated) {
    dotnet outdated
  }

  dotnet build --configuration "$Configuration" --no-restore
  Stop-OnError

  if (-not $SkipTest) {
    # Run tests, gather coverage
    dotnet test "$testProjectFolder" `
      --configuration "$Configuration" `
      --no-build `
      --results-directory './artifacts/testResults' `
      --logger 'trx' `
      --collect:'XPlat Code Coverage'

    Stop-OnError

    if (-not $SkipTestReport) {
      # Generate code coverage report
      dotnet reportgenerator `
        '-reports:./artifacts/testResults/*/coverage.cobertura.xml' `
        '-targetdir:./artifacts/testCoverage' `
        '-reporttypes:HtmlInline_AzurePipelines'

      Stop-OnError
    }
  }

  if (-not $SkipPublish) {
    # Package build
    dotnet publish "$webProjectFolder" --configuration "$Configuration" --no-build --output './artifacts/dist/release'
    Stop-OnError
  }

  $projectVersion = (dotnet nbgv get-version -f json | ConvertFrom-Json).NuGetPackageVersion
  Write-Output "`nBuilt $projectName $projectVersion`n"

} finally {
  Pop-Location
}
