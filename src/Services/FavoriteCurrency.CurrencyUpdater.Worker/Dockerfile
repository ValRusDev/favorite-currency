FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["FavoriteCurrency.sln", "./"]

COPY ["src/BuildingBlocks/FavoriteCurrency.SharedKernel/FavoriteCurrency.SharedKernel.csproj", "src/BuildingBlocks/FavoriteCurrency.SharedKernel/"]
COPY ["src/BuildingBlocks/FavoriteCurrency.SharedContracts/FavoriteCurrency.SharedContracts.csproj", "src/BuildingBlocks/FavoriteCurrency.SharedContracts/"]
COPY ["src/BuildingBlocks/FavoriteCurrency.SharedInfrastructure/FavoriteCurrency.SharedInfrastructure.csproj", "src/BuildingBlocks/FavoriteCurrency.SharedInfrastructure/"]

COPY ["src/Services/FavoriteCurrency.CurrencyUpdater.Worker/FavoriteCurrency.CurrencyUpdater.Worker.csproj", "src/Services/FavoriteCurrency.CurrencyUpdater.Worker/"]

RUN dotnet restore "src/Services/FavoriteCurrency.CurrencyUpdater.Worker/FavoriteCurrency.CurrencyUpdater.Worker.csproj"

COPY . .

WORKDIR "/src/src/Services/FavoriteCurrency.CurrencyUpdater.Worker"
RUN dotnet publish "FavoriteCurrency.CurrencyUpdater.Worker.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/runtime:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "FavoriteCurrency.CurrencyUpdater.Worker.dll"]