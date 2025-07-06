# Etapa de compilación
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia el archivo .csproj y restaura dependencias
COPY ["OlivarBackend.csproj", "./"]
RUN dotnet restore "OlivarBackend.csproj"

# Copia todo el proyecto
COPY . .

# Publicar el proyecto en modo Release
RUN dotnet publish "OlivarBackend.csproj" -c Release -o /app/publish

# Etapa final
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copia los archivos publicados
COPY --from=build /app/publish .

# Render usa la variable PORT
ENV ASPNETCORE_URLS=http://+:10000
EXPOSE 10000

ENTRYPOINT ["dotnet", "OlivarBackend.dll"]
