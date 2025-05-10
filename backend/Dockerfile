FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
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
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /out .
ENV ASPNETCORE_URLS=http://+:5000
ENTRYPOINT ["dotnet", "CocktailDebacle.API.dll"]