# FoodStuffs
A web application for managing recipes based on .Net Core 2.0 and Vue.js 2.0.

## Choose a Database
Within Web/Startup.cs, you can either... 
- Use a real database:
    1. Build a SQL Server database using the scripts in FoodStuffs.Data/Scripts
    2. Set a connection string in an appsettings.{environment}.json file.

- Use a volatile in-memory database
    1. Use "In-Memory" as your connection string within the appsettings.{environment}.json file.

## Get some tools
- [.Net Core SDK ^2.1.200](https://www.microsoft.com/net/download)
- [Yarn ^1.6.0](https://yarnpkg.com/lang/en/docs/install/)

## Clone, Build, And Run

#### Run a production version
```
git clone https://github.com/void-type/foodstuffs.git
cd foodstuffs
dotnet restore
dotnet build
cd FoodStuffs.Web/ClientApp
yarn
yarn build
cd ../
dotnet run --launch-profile "Kestrel (Production)"
```

#### Run development version (with webpack-dev-server)
```
git clone https://github.com/void-type/foodstuffs.git
cd foodstuffs
git checkout dev
dotnet restore
dotnet build
cd FoodStuffs.Web/ClientApp
yarn
yarn build
yarn serve
# in a second terminal:
dotnet run --launch-profile "Kestrel (Development)"
```
Note: This sets ASPNETCORE_ENVIRONMENT to "Development" and serves client assets from webpack-dev-server. This is a trade off of stability since not all assets are fetched from that server, versus rapid development since webpack gives us hot reloading. The other environment profiles will run a production build of the SPA from wwwroot.
