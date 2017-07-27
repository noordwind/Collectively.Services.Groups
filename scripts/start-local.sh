#!/bin/bash
export ASPNETCORE_ENVIRONMENT=local
cd src/Collectively.Services.Groups
dotnet run --urls "http://*:10007"