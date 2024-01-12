#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["./src/OdontoControl.UI/OdontoControl.UI.csproj", "/src/OdontoControl.UI/"]
COPY ["./src/OdontoControl.Core/OdontoControl.Core.csproj", "/src/OdontoControl.Core/"]
COPY ["./src/OdontoControl.Infrastructure/OdontoControl.Infrastructure.csproj", "/src/OdontoControl.Infrastructure/"]
RUN dotnet restore "/src/OdontoControl.UI/OdontoControl.UI.csproj"
COPY ./src /src
WORKDIR "/src/OdontoControl.UI"
RUN dotnet build "./OdontoControl.UI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./OdontoControl.UI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OdontoControl.UI.dll"]