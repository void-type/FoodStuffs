FROM microsoft/aspnetcore-build:2.0 AS build-env
WORKDIR /app

# copy everything and restore
COPY ../ ./
RUN dotnet restore && dotnet publish FoodStuffs.Web -c Release -o out

RUN dotnet publish -c Release -o out

# build runtime image
FROM microsoft/aspnetcore:2.0
WORKDIR /app
COPY --from=build-env /app/FoodStuffs.Web/out .
ENTRYPOINT ["dotnet", "FoodStuffs.Web.dll"]

# docker build -t foodstuffs-prod .
# docker run --name foodstuffs-prod foodstuffs-prod 