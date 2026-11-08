FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /source

# Install Node in the build container
RUN curl -sL https://deb.nodesource.com/setup_24.x | bash - \
  && apt-get update \
  && apt-get install -y nodejs

# pnpm switches to the version pinned in package.json's packageManager field
RUN npm install -g pnpm

# Copy build scripts first
COPY ./build/ ./build

# Copy files that will restore dependencies.
COPY ./*.slnx ./

COPY ./.config/ ./.config
COPY ./src/*/*.csproj ./
RUN for file in $(ls *.csproj); do mkdir -p ./src/${file%.*}/ && mv $file ./src/${file%.*}/; done

COPY ./tests/*/*.csproj ./
RUN for file in $(ls *.csproj); do mkdir -p ./tests/${file%.*}/ && mv $file ./tests/${file%.*}/; done

COPY ./Directory.Build.props ./
COPY ./Directory.Packages.props ./
COPY ./src/Directory.Build.props ./src/

COPY ./src/ClientApp/package.json ./src/ClientApp/
COPY ./src/ClientApp/pnpm-lock.yaml ./src/ClientApp/
COPY ./src/ClientApp/pnpm-workspace.yaml ./src/ClientApp/

# Restore dependencies.
RUN cd ./src/ClientApp && pnpm install --frozen-lockfile
RUN dotnet restore

# Copy everything to the build container
COPY ./ ./

# Build the app
RUN pwsh ./build/build.ps1

# Copy output from the build container to the run container
ARG ENTRY_POINT

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app

COPY --from=build /source/artifacts/dist/release .

ENTRYPOINT ["dotnet", "${ENTRY_POINT}.dll"]
