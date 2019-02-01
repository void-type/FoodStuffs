. ./util.ps1

Push-Location -Path "$webClientProjectFolder"
npm install
npm run build -- --mode "development"
Pop-Location
