FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

# Copy solution and restore dependencies
COPY Terranova.sln ./
COPY CocktailDebacle.API/*.csproj ./CocktailDebacle.API/
RUN dotnet restore

# Copy the rest of the application and build
COPY . ./
WORKDIR /app/CocktailDebacle.API
RUN dotnet publish -c Release -o /out

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /out .
ENTRYPOINT ["dotnet", "CocktailDebacle.API.dll"]