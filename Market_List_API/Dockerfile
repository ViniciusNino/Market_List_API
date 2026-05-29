# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar csproj e restaurar dependências
COPY ["MarketList_API/MarketList_API.csproj", "MarketList_API/"]
COPY ["MarketList_Business/MarketList_Business.csproj", "MarketList_Business/"]
COPY ["MarketList_Data/MarketList_Data.csproj", "MarketList_Data/"]
COPY ["MarketList_Model/MarketList_Model.csproj", "MarketList_Model/"]
COPY ["MarketList_Repository/MarketList_Repository.csproj", "MarketList_Repository/"]
RUN dotnet restore "MarketList_API/MarketList_API.csproj"

# Copiar todo o código e compilar
COPY . .
WORKDIR "/src/MarketList_API"
RUN dotnet build "MarketList_API.csproj" -c Release -o /app/build

# Stage 2: Publish
FROM build AS publish
RUN dotnet publish "MarketList_API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 3: Runtime Final
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 80
EXPOSE 443
ENTRYPOINT ["dotnet", "MarketList_API.dll"]
