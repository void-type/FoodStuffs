. ./util.ps1

Push-Location -Path "$webClientProjectFolder"
npm run test:unit
Pop-Location
