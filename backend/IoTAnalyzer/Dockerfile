FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["IoTAnalyzer.API/IoTAnalyzer.API.csproj", "IoTAnalyzer.API/"]
COPY ["IoTAnalyzer.UseCases/IoTAnalyzer.UseCases.csproj", "IoTAnalyzer.UseCases/"]
COPY ["IoTAnalyzer.Infrastructure/IoTAnalyzer.Infrastructure.csproj", "IoTAnalyzer.Infrastructure/"]
COPY ["IoTAnalyzer.Core/IoTAnalyzer.Core.csproj", "IoTAnalyzer.Core/"]
RUN dotnet restore "IoTAnalyzer.API/IoTAnalyzer.API.csproj"
COPY . .
RUN dotnet build "IoTAnalyzer.API/IoTAnalyzer.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "IoTAnalyzer.API/IoTAnalyzer.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
ENV ASPNETCORE_ENVIRONMENT=Production \
    ASPNETCORE_HTTP_PORTS=8080 \
    ASPNETCORE_FORWARDEDHEADERS_ENABLED=true
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IoTAnalyzer.API.dll"]
