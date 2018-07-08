#! /bin/pwsh-preview

Push-Location -Path "../"
docker build --build-arg env=Staging -t foodstuffs-staging .
Pop-Location
