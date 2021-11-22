Push-Location -Path "$PSScriptRoot/../"
. ./build/util.ps1
Pop-Location

Push-Location -Path "$PSScriptRoot/../$webClientProjectFolder"

try {
  npm install
  npm run build -- --watch --mode "development"

} finally {
  Pop-Location
}
