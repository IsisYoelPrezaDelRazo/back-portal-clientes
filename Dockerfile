# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Configurar fuentes de NuGet y limpiar caché
RUN dotnet nuget locals all --clear

# Copiamos los archivos del proyecto y configuración de NuGet
COPY ["pclienteDynamic_back.csproj", "./"]
COPY ["nuget.config", "./"]

# Restaurar con reintentos y configuración más robusta
RUN dotnet restore "./pclienteDynamic_back.csproj" \
    --verbosity normal \
    --no-cache \
    --force \
    --source https://api.nuget.org/v3/index.json

# Copiamos el resto del código y construimos
COPY . .
RUN dotnet publish "./pclienteDynamic_back.csproj" -c Release -o /app/publish

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

# Exponemos el puerto que usa tu aplicación (por defecto es 80 o 5000/5001)
EXPOSE 5152

ENTRYPOINT ["dotnet", "pclienteDynamic_back.dll"]
