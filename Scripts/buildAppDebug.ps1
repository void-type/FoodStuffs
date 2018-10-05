Push-Location -Path "../FoodStuffs.Web/ClientApp"
npm install
npm run build --mode "development"
Pop-Location

Push-Location -Path "../FoodStuffs.Web"
dotnet build --configuration "Debug"
Pop-Location
