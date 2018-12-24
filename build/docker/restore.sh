#! /bin/bash

# Restore client dependencies
pushd "../../src/FoodStuffs.Web/ClientApp"
npm install
popd

# Restore server dependencies
pushd "../../"
dotnet restore
popd
