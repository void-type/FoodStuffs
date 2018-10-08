. ./util.ps1

# Built the test assembly
Push-Location -Path "$testProjectFolder"
dotnet build --configuration "Debug"
Stop-OnError

# Generate code coverage
coverlet "./bin/Debug/netcoreapp2.1/$projectName.Test.dll" --target "dotnet" --targetargs "test --no-build" --format "opencover" --output "./coveragereport/coverage.opencover.xml"
Stop-OnError

# Generate code coverage report
reportgenerator "-reports:coveragereport/coverage.opencover.xml" "-targetdir:coveragereport"
Pop-Location
