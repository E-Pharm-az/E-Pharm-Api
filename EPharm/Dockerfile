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
COPY ["EPharm.Api/appsettings.Development.json", "./"]

ARG CONNECTION_STRING
ARG USER_CONNECTION_STRING
ARG SUPER_ADMIN_EMAIL
ARG SUPER_ADMIN_PASSWORD
ARG SUPER_ADMIN_FIRST_NAME
ARG SUPER_ADMIN_LAST_NAME
ARG SUPER_ADMIN_USER_NAME
ARG SUPER_ADMIN_FIN
ARG JWT_ISSUER
ARG JWT_AUDIENCE
ARG JWT_EXPIRATION_MINUTES
ARG JWT_KEY
ARG ACCESS_TOKEN_EXPIRATION
ARG REFRESH_TOKEN_EXPIRATION

ENV ConnectionStrings__DefaultConnection=$CONNECTION_STRING \
    ConnectionStrings__UserDefaultConnection=$USER_CONNECTION_STRING \
    SuperAdmin__Email=$SUPER_ADMIN_EMAIL \
    SuperAdmin__Password=$SUPER_ADMIN_PASSWORD \
    SuperAdmin__FirstName=$SUPER_ADMIN_FIRST_NAME \
    SuperAdmin__LastName=$SUPER_ADMIN_LAST_NAME \
    SuperAdmin__UserName=$SUPER_ADMIN_USER_NAME \
    SuperAdmin__Fin=$SUPER_ADMIN_FIN \
    JwtSettings__Issuer=$JWT_ISSUER \
    JwtSettings__Audience=$JWT_AUDIENCE \
    JwtSettings__ExpirationMinutes=$JWT_EXPIRATION_MINUTES \
    JwtSettings__Key=$JWT_KEY \
    ExpireSettings__AccessTokenExpiration=$ACCESS_TOKEN_EXPIRATION \
    ExpireSettings__RefreshTokenExpiration=$REFRESH_TOKEN_EXPIRATION


ENTRYPOINT ["dotnet", "EPharm.Api.dll"]
