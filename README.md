# FoodStuffs

[![License](https://img.shields.io/github/license/void-type/FoodStuffs.svg?style=flat-square)](https://github.com/void-type/FoodStuffs/blob/master/LICENSE.txt)
[![Build Status](https://img.shields.io/azure-devops/build/void-type/FoodStuffs/4.svg?style=flat-square)](https://dev.azure.com/void-type/FoodStuffs/_build/latest?definitionId=4&branchName=master)
[![Test Coverage](https://img.shields.io/azure-devops/coverage/void-type/FoodStuffs/4.svg?style=flat-square)](https://dev.azure.com/void-type/FoodStuffs/_build/latest?definitionId=4&branchName=master)

A web application for managing recipes based on .Net Core 2.1 and Vue.js 2.5.

This application demonstrates the [VoidCore](https://github.com/void-type/VoidCore) libraries.

## Features

* Printer-friendly views.
* Category tags on recipes.
* Text search on recipe names and categories.
* View history.
* Full logging on the server.
* No style dependencies. All custom CSS.
* See screenshots [here](docs/screenshots.md).

Not implemented yet:

* Users and authentication
* Side-by-side recipe viewing
* Recipe copying/cloning

## Make a Database

1. Build a SQL Server database using the migration scripts in /build/sql
2. Create and setup an appsettings.{environment}.json file.

## Get some tools

* [.Net Core SDK ^2.1.402](https://www.microsoft.com/net/download)
* [Node ^8.11.0](https://nodejs.org/en/)
* [Docker ^18.00 (optional)](https://docker.com)

Run /build/installAndUpdateTools.ps1 to get dotnet global tools needed for building this app.

## Local build

You will find everything you need to build and test this project in the /build folder.

There are also VSCode tasks for most scripts.

The following will build a production version of the application.

```powershell
cd build/
./build.ps1
```

## Docker multi-stage build

You don't need .Net or Node locally to run this application in [Docker](https://www.docker.com/).

```powershell
cd build/
./dockerBuild-Staging.ps1
./dockerRun-Staging.ps1
```
