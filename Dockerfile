# Etapa 1: Compilar
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiamos los archivos de solución y proyecto
COPY *.sln .
COPY OlivarBackend/*.csproj ./OlivarBackend/
RUN dotnet restore

# Copiar todo el contenido
COPY . .
WORKDIR /src/OlivarBackend
RUN dotnet publish -c Release -o /app/publish

# Etapa 2: Ejecutar
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 80
ENTRYPOINT ["dotnet", "OlivarBackend.dll"]
