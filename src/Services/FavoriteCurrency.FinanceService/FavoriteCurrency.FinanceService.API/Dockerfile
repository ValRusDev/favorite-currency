FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["FavoriteCurrency.sln", "./"]

COPY ["src/BuildingBlocks/FavoriteCurrency.SharedKernel/FavoriteCurrency.SharedKernel.csproj", "src/BuildingBlocks/FavoriteCurrency.SharedKernel/"]
COPY ["src/BuildingBlocks/FavoriteCurrency.SharedContracts/FavoriteCurrency.SharedContracts.csproj", "src/BuildingBlocks/FavoriteCurrency.SharedContracts/"]
COPY ["src/BuildingBlocks/FavoriteCurrency.SharedInfrastructure/FavoriteCurrency.SharedInfrastructure.csproj", "src/BuildingBlocks/FavoriteCurrency.SharedInfrastructure/"]

COPY ["src/Services/FavoriteCurrency.FinanceService/FavoriteCurrency.FinanceService.Domain/FavoriteCurrency.FinanceService.Domain.csproj", "src/Services/FavoriteCurrency.FinanceService/FavoriteCurrency.FinanceService.Domain/"]
COPY ["src/Services/FavoriteCurrency.FinanceService/FavoriteCurrency.FinanceService.Application/FavoriteCurrency.FinanceService.Application.csproj", "src/Services/FavoriteCurrency.FinanceService/FavoriteCurrency.FinanceService.Application/"]
COPY ["src/Services/FavoriteCurrency.FinanceService/FavoriteCurrency.FinanceService.Infrastructure/FavoriteCurrency.FinanceService.Infrastructure.csproj", "src/Services/FavoriteCurrency.FinanceService/FavoriteCurrency.FinanceService.Infrastructure/"]
COPY ["src/Services/FavoriteCurrency.FinanceService/FavoriteCurrency.FinanceService.API/FavoriteCurrency.FinanceService.API.csproj", "src/Services/FavoriteCurrency.FinanceService/FavoriteCurrency.FinanceService.API/"]

RUN dotnet restore "src/Services/FavoriteCurrency.FinanceService/FavoriteCurrency.FinanceService.API/FavoriteCurrency.FinanceService.API.csproj"

COPY . .

WORKDIR "/src/src/Services/FavoriteCurrency.FinanceService/FavoriteCurrency.FinanceService.API"
RUN dotnet publish "FavoriteCurrency.FinanceService.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "FavoriteCurrency.FinanceService.API.dll"]