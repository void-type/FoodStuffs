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
Pop-Location
Stop-OnError

# Build solution
Push-Location -Path "../"
dotnet build --configuration "Release"
Stop-OnError
Pop-Location

# Run tests, generate code coverage report
Push-Location -Path "$testProjectFolder"
dotnet test --configuration "Release" --no-build /p:CollectCoverage=true /p:CoverletOutputFormat=opencover /p:CoverletOutput="../../coverage/coverage.opencover.xml"
Stop-OnError
Pop-Location

# Generate code coverage report
Push-Location -Path "../coverage"
reportgenerator "-reports:coverage.opencover.xml" "-targetdir:."
Stop-OnError
Pop-Location

# Package build
Push-Location -Path "$webProjectFolder"
# Uncomment the self-contained and runtime args if we cannot update the server-wide framework.
dotnet publish --configuration "Release" --no-build --output "../../artifacts" #--self-contained --runtime "win-x64"
Pop-Location
