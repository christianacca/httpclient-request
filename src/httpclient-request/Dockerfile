FROM mcr.microsoft.com/dotnet/core/runtime:2.2 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /src
COPY httpclient-request.csproj .
RUN dotnet restore
COPY . .
RUN dotnet build -c Release --no-restore

FROM build AS publish
RUN dotnet publish -c Release -o /app --no-build

FROM base AS final
WORKDIR /app
ENTRYPOINT ["dotnet", "client.dll"]
COPY --from=publish /app .
