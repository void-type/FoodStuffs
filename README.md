# FoodStuffs
A web application for managing recipes.

## Setup

### Need
- .Net Core SDK
- Bower

### Build
```
git clone https://github.com/void-type/foodstuffs.git
cd foodstuffs
dotnet restore
cd FoodStuffs.Web
bower install
cd ../
dotnet build FoodStuffs.sln
```

### Database
Within Web/Startup.cs, you can either... 
- Use a real database:
    1. build a database using the scripts in FoodStuffs.Data/Sql 
    2. set a connection string in configuration
- Use a volitile in-memory database

### Run
```
cd FoodStuffs.Web
dotnet run
```