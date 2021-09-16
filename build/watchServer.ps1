Push-Location -Path "$PSScriptRoot/../"
. ./build/util.ps1

try {
  dotnet watch run --project "$webProjectFolder" --configuration 'Debug' --launch-profile 'Kestrel (Development)'

} finally {
  Pop-Location
}
