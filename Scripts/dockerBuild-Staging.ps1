#! /bin/pwsh-preview

Push-Location -Path "../"
docker build --pull --build-arg env=Staging --tag foodstuffs-staging .
Pop-Location
