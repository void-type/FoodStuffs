$originalLocation = Get-Location
$projectRoot = "$PSScriptRoot/../"

try {
  Set-Location -Path $projectRoot
  . ./build/buildSettings.ps1

  dotnet tool restore
  $projectVersion = (dotnet nbgv get-version -f json | ConvertFrom-Json).NuGetPackageVersion

  $entryPoint = $webProjectFolder -split '/' |
    Select-Object -Last 1

  docker build `
    --tag "void-type/$($entryPoint.ToLower()):$($projectVersion)" `
    --pull `
    --build-arg ENTRY_POINT=$entryPoint `
    .

} finally {
  Set-Location $originalLocation
}
