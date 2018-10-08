. ./util.ps1

Push-Location -Path "$webClientProjectFolder"
npm run lint
Pop-Location
