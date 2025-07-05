# Etapa de compilación
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY *.sln .
COPY . .   # Copia todo si el .csproj está en la raíz

RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish

# Etapa final
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "OlivarBackend.dll"]
