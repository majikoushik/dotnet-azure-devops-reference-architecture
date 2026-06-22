FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY EnterpriseClaims.sln ./
COPY src/Services/Claims.Api/Claims.Api.csproj src/Services/Claims.Api/
COPY src/Shared/BuildingBlocks/EnterpriseClaims.BuildingBlocks.csproj src/Shared/BuildingBlocks/
COPY src/Shared/Contracts/EnterpriseClaims.Contracts.csproj src/Shared/Contracts/
RUN dotnet restore src/Services/Claims.Api/Claims.Api.csproj
COPY . .
RUN dotnet publish src/Services/Claims.Api/Claims.Api.csproj -c Release -o /app/publish --no-restore

FROM runtime AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Claims.Api.dll"]
