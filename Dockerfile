FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /source

COPY global.json Directory.Build.props Directory.Packages.props ./
COPY src/ReserveHub.Domain/ReserveHub.Domain.csproj src/ReserveHub.Domain/
COPY src/ReserveHub.Application/ReserveHub.Application.csproj src/ReserveHub.Application/
COPY src/ReserveHub.Infrastructure/ReserveHub.Infrastructure.csproj src/ReserveHub.Infrastructure/
COPY src/ReserveHub.Api/ReserveHub.Api.csproj src/ReserveHub.Api/
RUN dotnet restore src/ReserveHub.Api/ReserveHub.Api.csproj

COPY src/ src/
RUN dotnet publish src/ReserveHub.Api/ReserveHub.Api.csproj -c Release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app .
USER $APP_UID
EXPOSE 8080
ENTRYPOINT ["dotnet", "ReserveHub.Api.dll"]
