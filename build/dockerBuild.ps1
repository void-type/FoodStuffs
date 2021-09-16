Push-Location -Path "$PSScriptRoot/../"
. ./build/util.ps1

try {
  dotnet tool restore
  $projectVersion = (dotnet nbgv get-version -f json | ConvertFrom-Json).NuGetPackageVersion

  docker build -t "$($projectName.ToLower()):$($projectVersion)" --pull .

} finally {
  Pop-Location
}
