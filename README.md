# FoodStuffs

[![License](https://img.shields.io/github/license/void-type/FoodStuffs.svg)](https://github.com/void-type/FoodStuffs/blob/main/LICENSE.txt)
[![Build Status](https://img.shields.io/azure-devops/build/void-type/VoidCore/18.svg)](https://dev.azure.com/void-type/VoidCore/_build/latest?definitionId=18&branchName=main)
[![Test Coverage](https://img.shields.io/azure-devops/coverage/void-type/VoidCore/18.svg)](https://dev.azure.com/void-type/VoidCore/_build/latest?definitionId=18&branchName=main)
[![ReleaseVersion](https://img.shields.io/github/release/void-type/FoodStuffs.svg)](https://github.com/void-type/FoodStuffs/releases)

A web application for managing recipes.

FoodStuffs is based on ASP.NET Core 6 and Vue.js 2.6.

This application demonstrates the [VoidCore](https://github.com/void-type/VoidCore) libraries.

## Features

* Printer-friendly views.
* Category tags on recipes.
* Multi-image uploads for recipes.
* Copy recipes.
* Text search on recipe names and categories.
* Recent history list.
* Full logging on the server.
* Bootstrap-Vue UI.
* See screenshots [here](docs/screenshots.md).

### Coming Soon™

* Users and authentication
* Side-by-side recipe viewing

## Build and Run FoodStuffs

### Make a Database

This project uses Entity Framework Database First.

1. Build a SQL Server database by running the migration scripts in /build/sql in order by date.

### Local build (production and development)

Install the following tools:

* [.NET SDK](https://www.microsoft.com/net/download)
* [Node](https://nodejs.org/en/)

See the /build folder for scripts used to test and build this project.

There are [VSCode](https://code.visualstudio.com/) tasks for each script. The build task (ctrl + shift + b) performs the standard CI build.

Run build.ps1 to make a production build.

```powershell
./build/build.ps1
```

Use the deployment/setup scripts as templates to deploy to your environment.

```powershell
./build/setupWebServer.ps1

./build/deployAppToProduction.ps1
```

### Docker multi-stage build

You don't need .NET or Node locally to run this application in [Docker](https://www.docker.com/).

```powershell
docker build
```
