$originalLocation = Get-Location
$projectRoot = "$PSScriptRoot/../"

try {
  Set-Location -Path $projectRoot
  . ./build/buildSettings.ps1

  dotnet tool restore
  $projectVersion = (dotnet nbgv get-version -f json | ConvertFrom-Json).NuGetPackageVersion

  docker build -t "$($projectName.ToLower()):$($projectVersion)" --pull .

} finally {
  Set-Location $originalLocation
}
