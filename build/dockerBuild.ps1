. ./util.ps1

Push-Location ../

docker build -t "$($projectName.ToLower()):$($projectVersion)" --pull .

Pop-Location
