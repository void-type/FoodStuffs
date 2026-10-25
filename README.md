# FoodStuffs

[![License](https://img.shields.io/github/license/void-type/FoodStuffs.svg)](https://github.com/void-type/FoodStuffs/blob/main/LICENSE.txt)
[![ReleaseVersion](https://img.shields.io/github/release/void-type/FoodStuffs.svg)](https://github.com/void-type/FoodStuffs/releases)
[![Build Status](https://github.com/void-type/FoodStuffs/actions/workflows/ci.yml/badge.svg?branch=main)](https://github.com/void-type/FoodStuffs/actions/workflows/ci.yml)

A web application for managing recipes.

FoodStuffs is based on ASP.NET Core and Vue.

This application demonstrates the [VoidCore](https://github.com/void-type/VoidCore) libraries.

## Features

- Meal cards to build weekly shopping list.
- Category tags on recipes. Appear as facets in search.
- Images for recipes with automatic cropping/compression.
- Copy recipes.
- Home page has random sort and infinite scroll for recipe discovery.
- Paginated and robust search backed by Lucene.
- Recent recipe history list.
- Unsaved change detection.
- Responsive UI using Bootstrap grid.
- Dark mode.
- Printer-friendly views.

## Build and Run

### Make a Database

This project uses Entity Framework Code First.

Pending migrations are applied automatically when the web app starts, including after deployment.

To create or update a database without starting the app, run `./build/dbApplyMigration.ps1`.

#### Database update history

Note that prior to v9, this project used Database First and SQL scripts.

v9 and onwards uses EF Code First with EF migrations, but there may be SQL scripts or migrator console apps that need to be run.

Check Git history for older migration tools.

### Local build (production and development)

Install the following tools:

- [.NET SDK](https://www.microsoft.com/net/download)
- [Node](https://nodejs.org/en/)

See the /build folder for scripts used to test and build this project.

There are [VSCode](https://code.visualstudio.com/) tasks for each script. The build task (ctrl + shift + b) performs the standard production CI build.

Run build.ps1 to make a production build.

```powershell
./build/build.ps1
```

CI runs on [GitHub Actions](https://github.com/void-type/FoodStuffs/actions/workflows/ci.yml). To tag and create a GitHub release, run the CI workflow manually on main. Deployment is done from a local build with the scripts below.

### IIS Deployment

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
