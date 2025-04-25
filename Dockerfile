FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore && dotnet publish -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /app .
COPY appsettings.Local.json .

# Esperar a que los servicios estén listos
HEALTHCHECK --interval=10s --timeout=3s --retries=3 \
  CMD curl --fail http://localhost:5000/health || exit 1

ENTRYPOINT ["dotnet", "MyBettingEdge.dll"]