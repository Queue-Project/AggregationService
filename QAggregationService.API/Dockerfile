FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 5007
EXPOSE 5008

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["LocalNuget/", "/src/LocalNuget/"]
COPY ["nuget.config", "/src/"]

COPY ["AggregationService/QAggregationService.API/QAggregationService.API.csproj", "QAggregationService.API/"]
COPY ["AggregationService/QAggregationService.Application/QAggregationService.Application.csproj", "QAggregationService.Application/"]
COPY ["AggregationService/QAggregationService.Contracts/QAggregationService.Contracts.csproj", "QAggregationService.Contracts/"]
RUN dotnet restore "QAggregationService.API/QAggregationService.API.csproj"
COPY . .
WORKDIR "/src/AggregationService/QAggregationService.API"
RUN dotnet build "./QAggregationService.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./QAggregationService.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "QAggregationService.API.dll"]
