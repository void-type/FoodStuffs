# FoodStuffs

A web application for managing recipes based on .Net Core 2.1 and Vue.js 2.0.

## Make a Database

    1. Build a SQL Server database using the migration scripts in /Scripts/sql
    2. Set a connection string in an appsettings.{environment}.json file.

## Get some tools

- [.Net Core SDK ^2.1.400](https://www.microsoft.com/net/download)
- [Yarn ^1.9.0](https://yarnpkg.com/lang/en/docs/install/)
- [Node ^8.11.0](https://nodejs.org/en/)
- [Docker ^18.00 (optional)](https://docker.com)

## Developers

You will find everything you need to build and test this project in the Scripts folder.

There is a script to install and update global tools used to develop this project.

There are also VSCode tasks for most scripts.

The following will build a production version of the application.

Note: VoidCore currently has no public build. You will have to build it manually and host it in a local Nuget feed. See the VoidCore readme on how to do this.

```powershell
cd Scripts/
./buildApp.ps1
```

## Docker Support with multistage build

Git clone and run the DockerFile. You don't need .Net or Yarn locally.
buildDocker-Staging.ps1 will build and run a Docker container on port 3333.
