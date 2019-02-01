[CmdletBinding()]
param(
  [string] $Configuration = "Release",
  [switch] $Quick
)

. ./util.ps1

# Clean coverage folder
Remove-Item -Path "../coverage" -Recurse -ErrorAction SilentlyContinue

# Clean testResults folder
Remove-Item -Path "../testResults" -Recurse -ErrorAction SilentlyContinue

# Run tests, gather coverage
Push-Location -Path "../tests/FoodStuffs.Test"

dotnet test `
  --configuration "$Configuration" `
  --no-build `
  --logger 'trx' `
  --results-directory '../../testResults' `
  /p:CollectCoverage=true `
  /p:CoverletOutputFormat=cobertura `
  /p:CoverletOutput="../../coverage/coverage.cobertura.xml"

Stop-OnError
Pop-Location

if ($Quick) {
  Exit $LASTEXITCODE
}

# Generate code coverage report
Push-Location -Path "../coverage"
reportgenerator "-reports:coverage.cobertura.xml" "-targetdir:." "-reporttypes:HtmlInline_AzurePipelines"
Stop-OnError
Pop-Location