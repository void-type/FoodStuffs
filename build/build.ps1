[CmdletBinding()]
param(
  [string] $Configuration = "Release"
)

. ./util.ps1

# Clean the artifacts folder
Remove-Item -Path "../artifacts" -Recurse -ErrorAction SilentlyContinue

# Clean coverage folder
Remove-Item -Path "../coverage" -Recurse -ErrorAction SilentlyContinue

# Clean testResults folder
Remove-Item -Path "../testResults" -Recurse -ErrorAction SilentlyContinue

# Lint, test and build client
Push-Location -Path "$webClientProjectFolder"
npm install
npm run lint
Stop-OnError
npm run test:unit
Stop-OnError
npm run build
Stop-OnError
node "tasks/set-version.js"
Stop-OnError
Pop-Location

# Build solution
Push-Location -Path "../"
dotnet build --configuration "$Configuration"
Stop-OnError
Pop-Location

# Run tests, gather coverage
Push-Location -Path "../tests/FoodStuffs.Test"

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

# Package build
Push-Location -Path "$webProjectFolder"
dotnet publish --configuration "$Configuration" --no-build --output "../../artifacts"
Stop-OnError
Pop-Location
