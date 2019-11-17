# FoodStuffs

[![License](https://img.shields.io/github/license/void-type/FoodStuffs.svg?style=flat-square)](https://github.com/void-type/FoodStuffs/blob/master/LICENSE.txt)
[![Build Status](https://img.shields.io/azure-devops/build/void-type/FoodStuffs/4.svg?style=flat-square)](https://dev.azure.com/void-type/FoodStuffs/_build/latest?definitionId=4&branchName=master)
[![Test Coverage](https://img.shields.io/azure-devops/coverage/void-type/FoodStuffs/4.svg?style=flat-square)](https://dev.azure.com/void-type/FoodStuffs/_build/latest?definitionId=4&branchName=master)
[![ReleaseVersion](https://img.shields.io/github/release/void-type/FoodStuffs.svg?style=flat-square)](https://github.com/void-type/FoodStuffs/releases)

A web application for managing recipes based on .Net Core 3.0 and Vue.js 2.6.

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

1. Build a SQL Server database using the migration scripts in /build/sql
2. Create and setup an appsettings.{environment}.json file.

### Get some tools

* [.Net Core SDK ^3.0.100](https://www.microsoft.com/net/download)
* [Node ^10.15.0](https://nodejs.org/en/)
* [Docker ^18.00 (optional)](https://docker.com)

### Local build

See the /build folder for scripts used to test and build this project. Run build.ps1 to make a production build.

```powershell
cd build
./build.ps1
```

There are [VSCode](https://code.visualstudio.com/) tasks for each script. The build task (ctrl + shift + b) performs the standard CI build.

### Docker multi-stage build

You don't need .Net or Node locally to run this application in [Docker](https://www.docker.com/).

```powershell
docker build
```
