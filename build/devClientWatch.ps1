. ./util.ps1

Push-Location -Path "$webClientProjectFolder"
npm install
npm run build -- --watch --mode "development"
Pop-Location
