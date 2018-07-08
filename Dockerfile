FROM microsoft/aspnetcore-build:2.0 AS build-env
WORKDIR /app

# Install Yarn and Node in the build container
ENV NODE_VERSION 8.11.2
ENV YARN_VERSION 1.7.0
ENV NODE_DOWNLOAD_SHA 67dc4c06a58d4b23c5378325ad7e0a2ec482b48cea802252b99ebe8538a3ab79
ENV NODE_DOWNLOAD_URL https://nodejs.org/dist/v$NODE_VERSION/node-v$NODE_VERSION-linux-x64.tar.gz

RUN curl -SL "$NODE_DOWNLOAD_URL" --output nodejs.tar.gz \
    && echo "$NODE_DOWNLOAD_SHA nodejs.tar.gz" | sha256sum -c - \
    && tar -xzf "nodejs.tar.gz" -C /usr/local --strip-components=1 \
    && rm nodejs.tar.gz \
    && npm i -g yarn@$YARN_VERSION \
    && ln -f -s /usr/local/bin/node /usr/local/bin/nodejs

# Optimize build by only copying files that will restore dependencies.
COPY ./*.sln ./
COPY ./*/*.csproj ./
RUN for file in $(ls *.csproj); do mkdir -p ./${file%.*}/ && mv $file ./${file%.*}/; done
COPY ./FoodStuffs.Web/ClientApp/package.json ./FoodStuffs.Web/ClientApp/
COPY ./FoodStuffs.Web/ClientApp/yarn.lock ./FoodStuffs.Web/ClientApp/

# Restore client dependencies.
RUN cd FoodStuffs.Web/ClientApp && \
    yarn

# Restore server dependencies
RUN dotnet restore

# Copy everything to the build container
COPY ./ ./

# Build the server app
RUN cd Scripts && \
    ./publishApp.sh

# Copy /out from the build container to the run container
FROM microsoft/aspnetcore:2.0
ARG env="Production"
WORKDIR /app
COPY --from=build-env /app/FoodStuffs.Web/out .
ENV ASPNETCORE_ENVIRONMENT=$env
ENTRYPOINT ["dotnet", "FoodStuffs.Web.dll"]
