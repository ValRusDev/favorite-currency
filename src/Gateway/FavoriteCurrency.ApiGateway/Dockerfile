FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["FavoriteCurrency.sln", "./"]

COPY ["src/BuildingBlocks/FavoriteCurrency.SharedKernel/FavoriteCurrency.SharedKernel.csproj", "src/BuildingBlocks/FavoriteCurrency.SharedKernel/"]
COPY ["src/BuildingBlocks/FavoriteCurrency.SharedContracts/FavoriteCurrency.SharedContracts.csproj", "src/BuildingBlocks/FavoriteCurrency.SharedContracts/"]
COPY ["src/BuildingBlocks/FavoriteCurrency.SharedInfrastructure/FavoriteCurrency.SharedInfrastructure.csproj", "src/BuildingBlocks/FavoriteCurrency.SharedInfrastructure/"]

COPY ["src/Gateway/FavoriteCurrency.ApiGateway/FavoriteCurrency.ApiGateway.csproj", "src/Gateway/FavoriteCurrency.ApiGateway/"]

RUN dotnet restore "src/Gateway/FavoriteCurrency.ApiGateway/FavoriteCurrency.ApiGateway.csproj"

COPY . .

WORKDIR "/src/src/Gateway/FavoriteCurrency.ApiGateway"
RUN dotnet publish "FavoriteCurrency.ApiGateway.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "FavoriteCurrency.ApiGateway.dll"]