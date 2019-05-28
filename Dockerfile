FROM mcr.microsoft.com/dotnet/core/sdk:3.0.100-preview5 AS build-env
WORKDIR /app

# Copy build scripts first
COPY ./build/ ./build

# Install Node in the build container
RUN cd ./build/docker && \
  ./installNode.sh

# Copy files that will restore dependencies.
COPY ./*.sln ./

COPY ./src/*/*.csproj ./
RUN for file in $(ls *.csproj); do mkdir -p ./src/${file%.*}/ && mv $file ./src/${file%.*}/; done

COPY ./tests/*/*.csproj ./
RUN for file in $(ls *.csproj); do mkdir -p ./tests/${file%.*}/ && mv $file ./tests/${file%.*}/; done

COPY ./src/FoodStuffs.Web/ClientApp/package.json ./src/FoodStuffs.Web/ClientApp/
COPY ./src/FoodStuffs.Web/ClientApp/package-lock.json ./src/FoodStuffs.Web/ClientApp/

# Restore dependencies.
RUN cd ./build/docker && \
  ./restore.sh

# Copy everything to the build container
COPY ./ ./

# Build the app
RUN cd ./build/ && \
  pwsh ./build.ps1

# Copy output from the build container to the run container
FROM mcr.microsoft.com/dotnet/core/aspnet:3.0.0-preview5
ARG env="Production"
WORKDIR /app
COPY --from=build-env /app/artifacts .
ENV ASPNETCORE_ENVIRONMENT=$env
ENTRYPOINT ["dotnet", "FoodStuffs.Web.dll"]
