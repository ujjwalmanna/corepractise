FROM mcr.microsoft.com/dotnet/core/sdk:2.1 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.1 AS build
WORKDIR /src
COPY *.sln ./
COPY SampleAppsAsMicroService/SampleAppsAsMicroService.csproj SampleAppsAsMicroService/
RUN dotnet restore
COPY . .
WORKDIR /src/SampleAppsAsMicroService
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "SampleAppsAsMicroService.dll"]
