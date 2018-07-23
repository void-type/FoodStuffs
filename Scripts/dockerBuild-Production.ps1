#! /bin/pwsh-preview

Push-Location -Path "../"
docker build --pull --build-arg env=Production -t foodstuffs-production .
Pop-Location
