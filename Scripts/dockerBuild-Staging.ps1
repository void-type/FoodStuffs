#! /bin/pwsh-preview

Push-Location -Path "../"
docker build --pull --build-arg env=Staging -t foodstuffs-staging .
Pop-Location
