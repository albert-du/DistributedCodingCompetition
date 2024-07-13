#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["DistributedCodingCompetition.LiveLeaders/DistributedCodingCompetition.LiveLeaders.csproj", "DistributedCodingCompetition.LiveLeaders/"]
RUN dotnet restore "./DistributedCodingCompetition.LiveLeaders/DistributedCodingCompetition.LiveLeaders.csproj"
COPY . .
WORKDIR "/src/DistributedCodingCompetition.LiveLeaders"
RUN dotnet build "./DistributedCodingCompetition.LiveLeaders.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./DistributedCodingCompetition.LiveLeaders.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DistributedCodingCompetition.LiveLeaders.dll"]