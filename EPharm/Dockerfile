FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

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

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "EPharm.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EPharm.Api.dll"]
