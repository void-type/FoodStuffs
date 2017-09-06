# FoodStuffs
A web application for managing recipes.

## Setup

### Database
Within Web/Startup.cs, you can either... 
- Use a real database:
    1. build a database using the scripts in FoodStuffs.Data/Sql 
    2. set a connection string in configuration
- Use a volitile in-memory database

### Need
- .Net Core SDK
- Bower

### Build And Run
```
git clone https://github.com/void-type/foodstuffs.git
cd foodstuffs
dotnet restore
dotnet build FoodStuffs.sln
cd FoodStuffs.Web
bower install
dotnet run
```