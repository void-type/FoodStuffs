. ./util.ps1

Push-Location -Path "$webClientProjectFolder"
npm install
npm run build -- --mode "development"
Pop-Location
Stop-OnError

Push-Location -Path "$webProjectFolder"
dotnet build --configuration "Debug"
Pop-Location
