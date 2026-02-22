# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy csproj and restore
COPY *.sln .
COPY MovieCatalogue.Api/*.csproj MovieCatalogue.Api/
RUN dotnet restore

# Copy everything else
COPY . .
RUN dotnet publish MovieCatalogue.Api -c Release -o /app/out

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "MovieCatalogue.Api.dll"]
