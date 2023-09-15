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

function Stop-OnError([string]$errorMessage) {
  if ($LASTEXITCODE -eq 0) {
    return
  }

  if (-not [string]::IsNullOrWhiteSpace($errorMessage)) {
    Write-Error $errorMessage
  }

  exit $LASTEXITCODE
}

$originalLocation = Get-Location
$projectRoot = "$PSScriptRoot/../"

try {
  Set-Location -Path $projectRoot
  . ./build/buildSettings.ps1

  # Clean the artifacts folders
  Remove-Item -Path './artifacts' -Recurse -ErrorAction SilentlyContinue

  # Restore local dotnet tools
  dotnet tool restore
  Stop-OnError

  # Lint and build client
  if (-not $SkipClient) {
    Set-Location -Path $webClientProjectFolder
    npm install --no-audit
    Stop-OnError

    if (-not $SkipFormat) {
      npm run lint
      Stop-OnError
      npm run format
      Stop-OnError "Formatting the client failed."
    }

    if (-not $SkipOutdated) {
      npm outdated
      npm audit --omit=dev
      Stop-OnError
    }

    npm run build
    Stop-OnError
  }

  # Build solution
  Set-Location -Path $projectRoot

  if (-not $SkipFormat) {
    # Don't stop build for TODOS
    dotnet format --verify-no-changes --exclude-diagnostics S1135
    Stop-OnError 'Please run formatter: dotnet format.'
  }

  dotnet restore
  Stop-OnError

  if (-not $SkipOutdated) {
    dotnet outdated
    dotnet list package --vulnerable --include-transitive
    Stop-OnError
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
  Set-Location $originalLocation
}
