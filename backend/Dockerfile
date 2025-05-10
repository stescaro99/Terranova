FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copia la soluzione e i file di progetto
COPY Terranova.sln ./
COPY CocktailDebacle.API/*.csproj ./CocktailDebacle.API/

# Ripristina le dipendenze
RUN --mount=type=cache,target=/root/.nuget/packages dotnet restore

# Copia tutto il codice sorgente
COPY CocktailDebacle.API ./CocktailDebacle.API

# Installa lo strumento dotnet-ef per gestire le migrazioni
RUN dotnet tool install --global dotnet-ef --version 9.0.3
ENV PATH="$PATH:/root/.dotnet/tools"

# Crea le migrazioni solo se non esistono
WORKDIR /app/CocktailDebacle.API
RUN if [ ! -d "./Migrations" ]; then \
    dotnet ef migrations add InitialCreate --project ./CocktailDebacle.API.csproj --startup-project ./CocktailDebacle.API.csproj; \
    fi

# Compila e pubblica il progetto
RUN --mount=type=cache,target=/root/.nuget/packages dotnet publish -c Release -o /out

# Immagine runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /out .
ENV ASPNETCORE_URLS=http://+:5000
ENTRYPOINT ["dotnet", "CocktailDebacle.API.dll"]