FROM mcr.microsoft.com/dotnet/runtime:10.0 AS runtime
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY EnterpriseClaims.slnx ./
COPY src/Workers/Notification.Worker/Notification.Worker.csproj src/Workers/Notification.Worker/
COPY src/Shared/BuildingBlocks/EnterpriseClaims.BuildingBlocks.csproj src/Shared/BuildingBlocks/
COPY src/Shared/Contracts/EnterpriseClaims.Contracts.csproj src/Shared/Contracts/
RUN dotnet restore src/Workers/Notification.Worker/Notification.Worker.csproj
COPY . .
RUN dotnet publish src/Workers/Notification.Worker/Notification.Worker.csproj -c Release -o /app/publish --no-restore

FROM runtime AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Notification.Worker.dll"]
