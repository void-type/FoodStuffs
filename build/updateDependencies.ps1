$originalLocation = Get-Location
$projectRoot = "$PSScriptRoot/../"

try {
  Set-Location -Path $projectRoot
  . ./build/buildSettings.ps1

  Set-Location -Path $webClientProjectFolder

  npx ncu -u --target minor
  Remove-Item -Force package-lock.json -ErrorAction SilentlyContinue
  Remove-Item -Recurse -Force node_modules -ErrorAction SilentlyContinue
  npm install --no-audit

  Set-Location -Path $projectRoot
  dotnet tool update --all
  dotnet outdated -u

  if ($LASTEXITCODE -ne 0) {
    Write-Warning "Some packages could not be updated automatically. Update them by hand in Directory.Packages.props."
    dotnet outdated
  }

} finally {
  Set-Location -Path $originalLocation
}
