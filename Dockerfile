FROM mcr.microsoft.com/dotnet/core/sdk:3.1.200 AS build-env
WORKDIR /app

# Install Node in the build container
RUN curl -sL https://deb.nodesource.com/setup_10.x | bash - \
  && apt-get update \
  && apt-get install -y nodejs

# Copy build scripts first
COPY ./build/ ./build

# Copy files that will restore dependencies.
COPY ./*.sln ./

COPY ./.config/ ./.config
COPY ./nuget.config ./

COPY ./src/*/*.csproj ./
RUN for file in $(ls *.csproj); do mkdir -p ./src/${file%.*}/ && mv $file ./src/${file%.*}/; done

COPY ./tests/*/*.csproj ./
RUN for file in $(ls *.csproj); do mkdir -p ./tests/${file%.*}/ && mv $file ./tests/${file%.*}/; done

COPY ./src/FoodStuffs.Web/ClientApp/package.json ./src/FoodStuffs.Web/ClientApp/
COPY ./src/FoodStuffs.Web/ClientApp/package-lock.json ./src/FoodStuffs.Web/ClientApp/

# Restore dependencies.
RUN cd ./src/FoodStuffs.Web/ClientApp && npm install
RUN dotnet restore

# Copy everything to the build container
COPY ./ ./

# Build the app
RUN cd ./build/ && \
  pwsh ./build.ps1

# Copy output from the build container to the run container
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
ARG env="Production"
WORKDIR /app
COPY --from=build-env /app/artifacts .
ENV ASPNETCORE_ENVIRONMENT=$env
ENTRYPOINT ["dotnet", "FoodStuffs.Web.dll"]/./
