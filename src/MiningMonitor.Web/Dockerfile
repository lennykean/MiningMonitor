FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

# Install Node.js
ENV NODE_VERSION 10.10.0
RUN curl -SL "https://nodejs.org/dist/v${NODE_VERSION}/node-v${NODE_VERSION}-linux-x64.tar.gz" --output nodejs.tar.gz \
    && tar -xzf "nodejs.tar.gz" -C /usr/local --strip-components=1 \
    && rm nodejs.tar.gz \
    && ln -s /usr/local/bin/node /usr/local/bin/nodejs

# Copy csproj files restore
FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["MiningMonitor.Web/MiningMonitor.Web.csproj", "/"]
RUN dotnet restore "MiningMonitor.Web.csproj"
COPY . .

# Install Node.js
ENV NODE_VERSION 10.10.0
RUN curl -SL "https://nodejs.org/dist/v${NODE_VERSION}/node-v${NODE_VERSION}-linux-x64.tar.gz" --output nodejs.tar.gz \
    && tar -xzf "nodejs.tar.gz" -C /usr/local --strip-components=1 \
    && rm nodejs.tar.gz \
    && ln -s /usr/local/bin/node /usr/local/bin/nodejs

# Build
WORKDIR "/src"
RUN dotnet build "MiningMonitor.Web.csproj" -c Release -o /app

# Publish
FROM build AS publish
RUN dotnet publish "MiningMonitor.Web.csproj" -c Release -o /app

# Build runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "MiningMonitor.Web.dll"]
