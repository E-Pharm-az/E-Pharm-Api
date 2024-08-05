# Stage 1: Base
FROM --platform=linux/amd64 mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Stage 2: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["EPharm.Api/EPharm.Api.csproj", "EPharm.Api/"]
COPY ["EPharm.Domain/EPharm.Domain.csproj", "EPharm.Domain/"]
COPY ["EPharm.Infrastructure/EPharm.Infrastructure.csproj", "EPharm.Infrastructure/"]
RUN dotnet restore "EPharm.Api/EPharm.Api.csproj"
COPY . .
WORKDIR "/src/EPharm.Api"
RUN dotnet build "EPharm.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Stage 3: Publish
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "EPharm.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Stage 4: Final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY EPharm.Api/Emails /app/Emails
ENTRYPOINT ["dotnet", "EPharm.Api.dll"]
