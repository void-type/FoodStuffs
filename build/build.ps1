[CmdletBinding()]
param(
  [string] $Configuration = "Release"
)

. ./util.ps1

# Clean the artifacts folder
Remove-Item -Path "../artifacts" -Recurse -ErrorAction SilentlyContinue

# Clean coverage folder
Remove-Item -Path "../coverage" -Recurse -ErrorAction SilentlyContinue

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

./testServer.ps1 -Configuration "$Configuration"
Stop-OnError

# Package build
Push-Location -Path "$webProjectFolder"
dotnet publish --configuration "$Configuration" --no-build --output "../../artifacts"
Stop-OnError
Pop-Location
