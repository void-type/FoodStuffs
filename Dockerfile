FROM microsoft/aspnetcore-build:2.0 AS build-env
WORKDIR /app

# copy everything and restore
COPY ./ ./
RUN dotnet restore && \
    dotnet publish FoodStuffs.Web -c Release -o FoodStuffs.Web/out

# build runtime image
FROM microsoft/aspnetcore:2.0
WORKDIR /app
COPY --from=build-env /app/FoodStuffs.Web/out .
ENTRYPOINT ["dotnet", "FoodStuffs.Web.dll"]

# docker build -t foodstuffs-prod .
# docker volume create webapplogs
# docker run -it --rm -p 5000:80 --name foodstuffs-prod --mount source=webapplogs,target=/webapplogs foodstuffs-prod