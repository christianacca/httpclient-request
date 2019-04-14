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

ARG COMMIT="local-build"
ARG DATE="1970-01-01T00:00:00Z"
ARG URL="https://github.com/christianacca/httpclient-request"
ARG VERSION="dirty"

WORKDIR /app
ENTRYPOINT ["dotnet", "httpclient-request.dll"]
COPY --from=publish /app .

LABEL org.opencontainers.image.created=$DATE \
    org.opencontainers.image.vendor="christianacca" \
    org.opencontainers.image.title="christianacca/httpclient-request" \
    org.opencontainers.image.description=".net core console application used to make http request using HttpClient created via HttpClientFactory" \
	org.opencontainers.image.licenses="MIT" \
    org.opencontainers.image.url="https://hub.docker.com/r/christianacca/httpclient-request" \
    org.opencontainers.image.version="$VERSION" \
    org.opencontainers.image.source=$URL \
    org.opencontainers.image.revision=$COMMIT