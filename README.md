# FoodStuffs
A web application for managing recipes.

## Choose a Database
Within Web/Startup.cs, you can either... 
- Use a real database:
    1. build a database using the scripts in Data/Sql 
    2. set a connection string in configuration (I typically use a user environment variable)
    3. Within Web/Startup.cs, ensure the FoodStuffsSqlData dependency is the only database uncommented.

- Use a volatile in-memory database
    1. Within Web/Startup.cs, ensure the MemoryFoodStuffsData dependency is the only database uncommented.

## Get some tools
- [.Net Core SDK 2.1.2](https://www.microsoft.com/net/download/thank-you/dotnet-sdk-2.1.2-windows-x64-installer)
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

#### Build the development version
```
git clone https://github.com/void-type/foodstuffs.git
cd foodstuffs
git checkout dev
dotnet restore
dotnet build FoodStuffs.sln
cd FoodStuffs.Web
yarn install
yarn run build
```
