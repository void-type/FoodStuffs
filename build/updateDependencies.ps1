$originalLocation = Get-Location
$projectRoot = "$PSScriptRoot/../"

try {
  Set-Location -Path $projectRoot
  . ./build/buildSettings.ps1

  Set-Location -Path $webClientProjectFolder

  # Newest versions allowed by the ranges in package.json and the release age in pnpm-workspace.yaml.
  pnpm update

  # Major versions are listed but not applied. Update them by hand with: pnpm update --latest --interactive
  pnpm outdated

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
