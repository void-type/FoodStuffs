<#
.SYNOPSIS
  Runs the browser regression suites in this folder against a running local FoodStuffs.

.DESCRIPTION
  Requires the app to be running (see the watch-web VS Code task) and `playwright-cli` on the PATH.
  The suites create records named "zz-e2e-<timestamp>" in whatever database the app uses and delete them
  again. Never point this at Test or Production.

.PARAMETER Suite
  Which suite to run. Defaults to all.

.PARAMETER BaseUrl
  The running app's URL.

.PARAMETER TimeoutMinutes
  How long to wait for each suite before giving up.
#>
[CmdletBinding()]
param(
  [ValidateSet('all', 'guard', 'images')]
  [string] $Suite = 'all',
  [string] $BaseUrl = 'https://localhost:5001',
  [int] $TimeoutMinutes = 10
)

$suites = [ordered]@{
  guard  = 'unsaved-changes-guard.js'
  images = 'recipe-images.js'
}

if (-not (Get-Command playwright-cli -ErrorAction SilentlyContinue)) {
  throw 'playwright-cli was not found on the PATH. Install it with: npm install -g @playwright/cli@latest'
}

$selected = if ($Suite -eq 'all') { $suites.Keys } else { @($Suite) }
$failed = $false

# playwright-cli writes snapshots and logs into the current directory, so keep them out of the repo.
Push-Location ([System.IO.Path]::GetTempPath())

try {
  foreach ($name in $selected) {
    $script = Join-Path $PSScriptRoot $suites[$name]
    Write-Host "== $name ($($suites[$name]))"

    # A fresh browser per suite, so a suite that stops mid-way can't leave a dirty page or dialog behind.
    playwright-cli close 2>&1 | Out-Null
    playwright-cli open $BaseUrl 2>&1 | Out-Null
    playwright-cli localstorage-delete e2eResults 2>&1 | Out-Null
    # run-code can return before the script ends (native dialogs make the CLI report a modal state),
    # so run it in the background and poll the results the script saves to localStorage.
    $job = Start-Job -ScriptBlock {
      param($file)
      playwright-cli run-code --filename=$file 2>&1 | Out-Null
    } -ArgumentList $script

    $results = ''
    $deadline = (Get-Date).AddMinutes($TimeoutMinutes)
    while ((Get-Date) -lt $deadline) {
      Start-Sleep -Seconds 5
      $results = (playwright-cli localstorage-get e2eResults 2>&1 | Out-String)
      if ($results -match '(?m)^DONE\s*$') { break }
    }

    Remove-Job $job -Force -ErrorAction SilentlyContinue

    $lines = ($results -replace '^e2eResults=', '') -split "`r?`n" | Where-Object { $_ -match '^(PASS|FAIL)' }
    $lines | ForEach-Object { Write-Host $_ -ForegroundColor $(if ($_ -like 'FAIL*') { 'Red' } else { 'DarkGreen' }) }

    $failures = @($lines | Where-Object { $_ -like 'FAIL*' }).Count
    $finished = $results -match '(?m)^DONE\s*$'
    Write-Host ("{0}: {1} passed, {2} failed{3}" -f $name, ($lines.Count - $failures), $failures, $(if ($finished) { '' } else { ' (timed out before finishing)' }))

    if ($failures -gt 0 -or -not $finished) { $failed = $true }
  }
} finally {
  playwright-cli close 2>&1 | Out-Null
  Pop-Location
}

if ($failed) { exit 1 }
