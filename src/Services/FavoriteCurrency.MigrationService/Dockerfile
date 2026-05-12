FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["FavoriteCurrency.sln", "./"]

COPY ["src/BuildingBlocks/FavoriteCurrency.SharedKernel/FavoriteCurrency.SharedKernel.csproj", "src/BuildingBlocks/FavoriteCurrency.SharedKernel/"]
COPY ["src/BuildingBlocks/FavoriteCurrency.SharedContracts/FavoriteCurrency.SharedContracts.csproj", "src/BuildingBlocks/FavoriteCurrency.SharedContracts/"]
COPY ["src/BuildingBlocks/FavoriteCurrency.SharedInfrastructure/FavoriteCurrency.SharedInfrastructure.csproj", "src/BuildingBlocks/FavoriteCurrency.SharedInfrastructure/"]

COPY ["src/Services/FavoriteCurrency.MigrationService/FavoriteCurrency.MigrationService.csproj", "src/Services/FavoriteCurrency.MigrationService/"]

RUN dotnet restore "src/Services/FavoriteCurrency.MigrationService/FavoriteCurrency.MigrationService.csproj"

COPY . .

WORKDIR "/src/src/Services/FavoriteCurrency.MigrationService"
RUN dotnet publish "FavoriteCurrency.MigrationService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/runtime:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "FavoriteCurrency.MigrationService.dll"]