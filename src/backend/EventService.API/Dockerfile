FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and project files
COPY EventPlatform.sln .
COPY EventService.API/EventService.API.csproj EventService.API/
COPY EventService.Application/EventService.Application.csproj EventService.Application/
COPY EventService.Domain/EventService.Domain.csproj EventService.Domain/
COPY EventService.Infrastructure/EventService.Infrastructure.csproj EventService.Infrastructure/
COPY Shared.Contracts/Shared.Contracts.csproj Shared.Contracts/
COPY NotificationService.API/NotificationService.API.csproj NotificationService.API/
COPY NotificationService.Application/NotificationService.Application.csproj NotificationService.Application/
COPY NotificationService.Domain/NotificationService.Domain.csproj NotificationService.Domain/
COPY NotificationService.Infrastructure/NotificationService.Infrastructure.csproj NotificationService.Infrastructure/

RUN dotnet restore EventService.API/EventService.API.csproj

# Copy source code
COPY . .

# Build
WORKDIR /src/EventService.API
RUN dotnet publish -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "EventService.API.dll"]
