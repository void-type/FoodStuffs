#! /bin/pwsh-preview

Push-Location -Path "../"
docker build --build-arg env=Production -t foodstuffs-production .
Pop-Location
