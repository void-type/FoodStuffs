function Stop-OnError {
    if ($LASTEXITCODE -ne 0) {
      Pop-Location
      Exit $LASTEXITCODE
    }
  }

  # Built the test assembly
  Push-Location -Path "../FoodStuffs.Test"
  dotnet build --configuration "Debug"
  Stop-OnError

  # Generate code coverage
  coverlet "./bin/Debug/netcoreapp2.1/FoodStuffs.Test.dll" --target "dotnet" --targetargs "test --no-build" --format "opencover" --output "./coveragereport/coverage.opencover.xml"
  Stop-OnError

  # Generate code coverage report
  reportgenerator "-reports:coveragereport/coverage.opencover.xml" "-targetdir:coveragereport"
  Pop-Location
