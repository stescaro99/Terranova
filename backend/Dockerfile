FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

COPY Terranova.sln ./
COPY CocktailDebacle.API/*.csproj ./CocktailDebacle.API/

RUN --mount=type=cache,target=/root/.nuget/packages dotnet restore

COPY CocktailDebacle.API ./CocktailDebacle.API

RUN dotnet tool install --global dotnet-ef --version 9.0.3
ENV PATH="$PATH:/root/.dotnet/tools"

WORKDIR /app/CocktailDebacle.API
RUN if [ ! -d "./Migrations" ]; then \
    dotnet ef migrations add InitialCreate --project ./CocktailDebacle.API.csproj --startup-project ./CocktailDebacle.API.csproj; \
    fi

RUN --mount=type=cache,target=/root/.nuget/packages dotnet publish -c Release -o /out

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /out .
ENV ASPNETCORE_URLS=http://+:5267
ENTRYPOINT ["dotnet", "CocktailDebacle.API.dll"]