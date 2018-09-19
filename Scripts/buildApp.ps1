function Stop-OnError {
  if ($LASTEXITCODE -ne 0) {
    Pop-Location
    Exit $LASTEXITCODE
  }
}

Push-Location -Path "../FoodStuffs.Web/ClientApp"
yarn
yarn run vue-cli-service lint
Stop-OnError
yarn run vue-cli-service build --modern
Pop-Location
Stop-OnError
./testServer.ps1
Stop-OnError
Push-Location -Path "../FoodStuffs.Web"
Remove-Item -Path "out" -Recurse -ErrorAction SilentlyContinue
# Uncomment the self-contained and runtime args if we cannot update the server-wide framework.
dotnet publish --configuration "Release" --output "out" #--self-contained --runtime "win-x64"
Pop-Location
