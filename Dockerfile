FROM mcr.microsoft.com/dotnet/core/sdk:3.0.100 AS build-env
WORKDIR /app

# Install 2.2 SDK for tools
ENV DOTNET_SDK_VERSION '2.2.402'

# linux-64
ENV DOTNET_SDK_ARCH 'linux-x64'
ENV DOTNET_SDK_SHA512 '81937de0874ee837e3b42e36d1cf9e04bd9deff6ba60d0162ae7ca9336a78f733e624136d27f559728df3f681a72a669869bf91d02db47c5331398c0cfda9b44'

# Raspberry Pi 3 - NBGV doesn't work yet
# ENV DOTNET_SDK_ARCH 'linux-arm'
# ENV DOTNET_SDK_SHA512 'B8F240ACFF5C0371CCFFA483172BD98EA2F202EB884B7AA0C244EFC8FF648193BB565470D51AB74AF56B293989F1D3030BF128CAEF2C8F1C31F30B999C92F244'

RUN curl -SL --output dotnet.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/Sdk/$DOTNET_SDK_VERSION/dotnet-sdk-$DOTNET_SDK_VERSION-$DOTNET_SDK_ARCH.tar.gz \
  && echo "$DOTNET_SDK_SHA512 dotnet.tar.gz" | sha512sum -c - \
  && tar -zxf dotnet.tar.gz -C /usr/share/dotnet --skip-old-files \
  && rm dotnet.tar.gz \
  && dotnet --info

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
FROM mcr.microsoft.com/dotnet/core/aspnet:3.0.0
ARG env="Production"
WORKDIR /app
COPY --from=build-env /app/artifacts .
ENV ASPNETCORE_ENVIRONMENT=$env
ENTRYPOINT ["dotnet", "FoodStuffs.Web.dll"]/./
