# Usar la imagen base de .NET 9 SDK para compilar
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiar archivos de proyectos
COPY ["src/MesaDeAyuda.Domain/MesaDeAyuda.Domain.csproj", "MesaDeAyuda.Domain/"]
COPY ["src/MesaDeAyuda.Data/MesaDeAyuda.Data.csproj", "MesaDeAyuda.Data/"]
COPY ["src/MesaDeAyuda.Api/MesaDeAyuda.Api.csproj", "MesaDeAyuda.Api/"]

# Restaurar dependencias del proyecto API
RUN dotnet restore "MesaDeAyuda.Api/MesaDeAyuda.Api.csproj"

# Copiar el resto del código fuente
COPY ["src/", "."]

# Compilar la aplicación
RUN dotnet build "MesaDeAyuda.Api/MesaDeAyuda.Api.csproj" -c Release -o /app/build

# Publicar la aplicación
FROM build AS publish
RUN dotnet publish "MesaDeAyuda.Api/MesaDeAyuda.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Usar la imagen base de .NET 9 runtime para ejecutar
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Instalar SQLite (necesario para el runtime)
RUN apt-get update && apt-get install -y sqlite3 libsqlite3-dev && rm -rf /var/lib/apt/lists/*

# Copiar la aplicación publicada
COPY --from=publish /app/publish .

# Crear directorio para la base de datos
RUN mkdir -p /app/App_Data

# Exponer el puerto
EXPOSE 8080

# Configurar variable de entorno para el puerto
ENV ASPNETCORE_URLS=http://+:8080

# Comando para ejecutar la aplicación
ENTRYPOINT ["dotnet", "MesaDeAyuda.Api.dll"]