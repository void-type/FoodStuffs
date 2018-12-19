# FoodStuffs

A web application for managing recipes based on .Net Core 2.1 and Vue.js 2.0.

WARNING - This application is still in early development.

## Make a Database

1. Build a SQL Server database using the migration scripts in /build/sql
2. Set a connection string in an appsettings.{environment}.json file.

## Get some tools

- [.Net Core SDK ^2.1.402](https://www.microsoft.com/net/download)
- [Node ^8.11.0](https://nodejs.org/en/)
- [Docker ^18.00 (optional)](https://docker.com)

Run build/installAndUpdateTools.ps1 to get dotnet global tools needed for building this app.

## Developers

You will find everything you need to build and test this project in the build folder.

There are also VSCode tasks for most scripts.

The following will build a production version of the application.

Note: VoidCore currently has no public build. You will have to build it manually and host it in a local Nuget feed before building FoodStuffs. See the VoidCore readme on how to do this.

```powershell
cd build/
./buildApp.ps1
```

## Docker Support with multi-stage build

You don't need .Net or Node locally to run this application in docker.

```powershell
cd build/
./dockerBuild-Staging.ps1
./dockerRun-Staging.ps1
```
