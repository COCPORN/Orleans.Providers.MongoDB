@echo off
dotnet build --configuration release "..\Orleans.Providers.MongoDB\Bember.Orleans.Providers.MongoDB.csproj"
dotnet pack --configuration release "..\Orleans.Providers.MongoDB\Bember.Orleans.Providers.MongoDB.csproj"