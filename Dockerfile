# -------- Build Stage --------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy solution file
COPY *.sln .

# Copy all project files
COPY MovieCatalogue.Api/*.csproj MovieCatalogue.Api/
COPY MovieCatalogue.DataImporter/*.csproj MovieCatalogue.DataImporter/
COPY MovieCatalogue.RankingTrainer/*.csproj MovieCatalogue.RankingTrainer/

# Restore dependencies
RUN dotnet restore

# Copy the rest of the source code
COPY . .

# Publish only the API project
RUN dotnet publish MovieCatalogue.Api -c Release -o /app/out

# -------- Runtime Stage --------
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build /app/out .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "MovieCatalogue.Api.dll"]
