FROM microsoft/dotnet:2.1-sdk AS build-env
WORKDIR /app

# Install Node in the build container
ENV NODE_VERSION 8.12.0
ENV NODE_DOWNLOAD_SHA 3df19b748ee2b6dfe3a03448ebc6186a3a86aeab557018d77a0f7f3314594ef6
ENV NODE_DOWNLOAD_URL https://nodejs.org/dist/v$NODE_VERSION/node-v$NODE_VERSION-linux-x64.tar.gz

RUN curl -SL "$NODE_DOWNLOAD_URL" --output nodejs.tar.gz \
    && echo "$NODE_DOWNLOAD_SHA nodejs.tar.gz" | sha256sum -c - \
    && tar -xzf "nodejs.tar.gz" -C /usr/local --strip-components=1 \
    && rm nodejs.tar.gz \
    && ln -f -s /usr/local/bin/node /usr/local/bin/nodejs

# Optimize build by only copying files that will restore dependencies.
COPY ./*.sln ./
COPY ./*/*.csproj ./
RUN for file in $(ls *.csproj); do mkdir -p ./${file%.*}/ && mv $file ./${file%.*}/; done
COPY ./FoodStuffs.Web/ClientApp/package.json ./FoodStuffs.Web/ClientApp/
COPY ./FoodStuffs.Web/ClientApp/package-lock.json ./FoodStuffs.Web/ClientApp/

# Restore client dependencies.
RUN cd FoodStuffs.Web/ClientApp && \
    npm install

# Restore server dependencies
RUN dotnet restore

# Copy everything to the build container
COPY ./ ./

# Build the server app
RUN cd Scripts && \
    ./publishApp.sh

# Copy /out from the build container to the run container
FROM microsoft/dotnet:2.1-aspnetcore-runtime
ARG env="Production"
WORKDIR /app
COPY --from=build-env /app/FoodStuffs.Web/out .
ENV ASPNETCORE_ENVIRONMENT=$env
ENTRYPOINT ["dotnet", "FoodStuffs.Web.dll"]
