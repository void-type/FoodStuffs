$originalLocation = Get-Location
$projectRoot = "$PSScriptRoot/../"

try {
  Set-Location -Path $projectRoot
  . ./build/buildSettings.ps1

  Set-Location -Path $webClientProjectFolder

  pnpm exec ncu -u --target minor --cooldown 3
  pnpm install
  pnpm update

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
