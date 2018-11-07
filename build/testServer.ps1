. ./util.ps1

Remove-Item -Path "../coverage" -Recurse -ErrorAction SilentlyContinue

# Build
Push-Location -Path "../"
dotnet build --configuration "Debug"
Stop-OnError
Pop-Location

# Run tests, generate code coverage report
Push-Location -Path "$testProjectFolder"
dotnet test --configuration "Debug" --no-build /p:CollectCoverage=true /p:CoverletOutputFormat=opencover /p:CoverletOutput="../../coverage/coverage.opencover.xml"
Stop-OnError
Pop-Location

# Generate code coverage report
Push-Location -Path "../coverage"
reportgenerator "-reports:coverage.opencover.xml" "-targetdir:."
Stop-OnError
Pop-Location
