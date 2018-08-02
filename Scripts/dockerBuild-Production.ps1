#! /bin/pwsh-preview

Push-Location -Path "../"
docker build --pull --build-arg env=Production --tag foodstuffs-production .
Pop-Location
