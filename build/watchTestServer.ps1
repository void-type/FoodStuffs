$originalLocation = Get-Location
$projectRoot = "$PSScriptRoot/../"

try {
  Set-Location -Path $projectRoot
  . ./build/buildSettings.ps1

  # Work around dotnet watch crashing on NBGV's shared PrivateP2PCaching.proj (dotnet/sdk#54552).
  $env:NBGV_CacheMode = 'None'

  dotnet watch test --project "$testProjectFolder" --configuration 'Debug'

} finally {
  Set-Location $originalLocation
}
