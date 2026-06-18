FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["LogisticsApp/LogisticsApp/LogisticsApp.csproj", "LogisticsApp/LogisticsApp/"]
RUN dotnet restore "LogisticsApp/LogisticsApp/LogisticsApp.csproj"

COPY . .
WORKDIR "/src/LogisticsApp/LogisticsApp"
RUN dotnet build "LogisticsApp.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "LogisticsApp.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LogisticsApp.dll"]