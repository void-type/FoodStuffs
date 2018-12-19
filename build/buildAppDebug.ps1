. ./util.ps1

Push-Location -Path "$webClientProjectFolder"
npm install
npm run build -- --mode "development"
Stop-OnError
Pop-Location

Push-Location -Path "$webProjectFolder"
dotnet build --configuration "Debug"
Pop-Location
