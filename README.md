# FoodStuffs
A web application for managing recipes based on .Net Core 2.0 and Vue.js 2.0.

## Make a Database
    1. Build a SQL Server database using the scripts in FoodStuffs.Data/Scripts
    2. Set a connection string in an appsettings.{environment}.json file.

## Get some tools
- [.Net Core SDK ^2.1.200](https://www.microsoft.com/net/download)
- [Yarn ^1.6.0](https://yarnpkg.com/lang/en/docs/install/)
- [Docker ^18.05 (optional)](https://docker.com)

## Clone, Build, And Run

#### Run a production version
```
git clone https://github.com/void-type/foodstuffs
cd foodstuffs
dotnet build
cd FoodStuffs.Web/ClientApp
yarn
yarn build
cd ../
dotnet run --launch-profile "Kestrel (Production)"
```

#### Run development version with file watching
```
git clone https://github.com/void-type/foodstuffs
cd foodstuffs/FoodStuffs.Web/ClientApp
yarn
yarn watch

# In another terminal
cd foodstuffs
dotnet watch run --launch-profile 'Kestrel (Development)'
```

#### VSCode tasks
There are various tasks set up in VSCode to build, test and watch files for rapid development.

## Docker Support with multistage build
Git clone and run the DockerFile. You don't need Dotnet or Yarn locally.