FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["FavoriteCurrency.sln", "./"]
COPY ["src/BuildingBlocks/FavoriteCurrency.SharedKernel/FavoriteCurrency.SharedKernel.csproj", "src/BuildingBlocks/FavoriteCurrency.SharedKernel/"]
COPY ["src/BuildingBlocks/FavoriteCurrency.SharedContracts/FavoriteCurrency.SharedContracts.csproj", "src/BuildingBlocks/FavoriteCurrency.SharedContracts/"]
COPY ["src/BuildingBlocks/FavoriteCurrency.SharedInfrastructure/FavoriteCurrency.SharedInfrastructure.csproj", "src/BuildingBlocks/FavoriteCurrency.SharedInfrastructure/"]
COPY ["src/Services/FavoriteCurrency.UserService/FavoriteCurrency.UserService.Domain/FavoriteCurrency.UserService.Domain.csproj", "src/Services/FavoriteCurrency.UserService/FavoriteCurrency.UserService.Domain/"]
COPY ["src/Services/FavoriteCurrency.UserService/FavoriteCurrency.UserService.Application/FavoriteCurrency.UserService.Application.csproj", "src/Services/FavoriteCurrency.UserService/FavoriteCurrency.UserService.Application/"]
COPY ["src/Services/FavoriteCurrency.UserService/FavoriteCurrency.UserService.Infrastructure/FavoriteCurrency.UserService.Infrastructure.csproj", "src/Services/FavoriteCurrency.UserService/FavoriteCurrency.UserService.Infrastructure/"]
COPY ["src/Services/FavoriteCurrency.UserService/FavoriteCurrency.UserService.API/FavoriteCurrency.UserService.API.csproj", "src/Services/FavoriteCurrency.UserService/FavoriteCurrency.UserService.API/"]

RUN dotnet restore "src/Services/FavoriteCurrency.UserService/FavoriteCurrency.UserService.API/FavoriteCurrency.UserService.API.csproj"

COPY . .
WORKDIR "/src/src/Services/FavoriteCurrency.UserService/FavoriteCurrency.UserService.API"
RUN dotnet publish "FavoriteCurrency.UserService.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "FavoriteCurrency.UserService.API.dll"]