. ./util.ps1

Push-Location -Path "$webClientProjectFolder"
npm install
npm run lint
Stop-OnError
npm run test:unit
Stop-OnError
npm run build
Pop-Location
Stop-OnError
./testServer.ps1
Stop-OnError
Push-Location -Path "$webProjectFolder"
Remove-Item -Path "out" -Recurse -ErrorAction SilentlyContinue
# Uncomment the self-contained and runtime args if we cannot update the server-wide framework.
dotnet publish --configuration "Release" --output "out" #--self-contained --runtime "win-x64"
Pop-Location
