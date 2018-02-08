# FoodStuffs
A web application for managing recipes based on .Net Core 2.0 and Vue.js 2.0.

## Choose a Database
Within Web/Startup.cs, you can either... 
- Use a real database:
    1. Build a SQL Server database using the scripts in FoodStuffs.Data/SqlScripts
    2. Set a connection string in an appSecrets.{environment}.json file.
    3. Within FoodStuffs.Web/Startup.cs, ensure the FoodStuffsSqlData registration is uncommented; comment the other providers.

- Use a volatile in-memory database
    1. Within FoodStuffs.Web/Startup.cs, ensure the MemoryFoodStuffsData dependency is the only database uncommented.

## Get some tools
- [.Net Core SDK 2.1.4](https://www.microsoft.com/net/download/thank-you/dotnet-sdk-2.1.4-windows-x64-installer)
- [Yarn](https://yarnpkg.com/lang/en/docs/install/)

## Clone, Build, And Run

#### Run a production version
```
git clone https://github.com/void-type/foodstuffs.git
cd foodstuffs
dotnet restore
dotnet build FoodStuffs.sln
cd FoodStuffs.Web
yarn install
yarn run build
dotnet run
```

#### Run development version (with webpack-dev-server)
```
git clone https://github.com/void-type/foodstuffs.git
cd foodstuffs
git checkout dev
dotnet restore
dotnet build FoodStuffs.sln
cd FoodStuffs.Web
yarn install
yarn run build
yarn run dev
dotnet run --launch-profile "Kestrel (Development)"
```
Note: This sets ASPNETCORE_ENVIRONMENT to "Development" and serves client assets from webpack-dev-server. This is a trade off of stability since not all assets are fetched from that server, versus rapid development since webpack gives us hot reloading. The other environment profiles will run a production build of the SPA from wwwroot.
