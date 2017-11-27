# FoodStuffs
A web application for managing recipes.

## Setup

### Database
Within Web/Startup.cs, you can either... 
- Use a real database:
    1. build a database using the scripts in Data/Sql 
    2. set a connection string in configuration
    3. Within Web/Startup.cs, uncomment the FoodStuffsSqlData dependency.

- Use a volatile in-memory database
    1. Within Web/Startup.cs, uncomment the MemoryFoodStuffsData dependency.

### Tools needed to build
- .Net Core SDK
- Yarn
- Webpack

### Clone, Build, And Run
```
git clone https://github.com/void-type/foodstuffs.git
cd foodstuffs
dotnet restore
dotnet build FoodStuffs.sln
cd FoodStuffs.Web
yarn install
./node_modules/.bin/webpack -p
dotnet run
```

Note: currently SCSS files are not compiled by webpack. Compile app.scss to wwwroot/dist/app.css and app.min.css manually. 