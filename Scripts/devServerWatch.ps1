Push-Location -Path "../FoodStuffs.Web"
dotnet watch run --configuration "Release" --launch-profile 'Kestrel (Development)'
Pop-Location
