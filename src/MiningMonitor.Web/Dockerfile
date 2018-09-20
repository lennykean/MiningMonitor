# Create runtime image
FROM microsoft/dotnet:2.1-aspnetcore-runtime as runtime
WORKDIR /app
EXPOSE 80

# Create build image
FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src

# Install Node.js in build container
ENV NODE_VERSION 8.11.2
RUN curl -SL "https://nodejs.org/dist/v${NODE_VERSION}/node-v${NODE_VERSION}-linux-x64.tar.gz" --output nodejs.tar.gz \
    && tar -xzf "nodejs.tar.gz" -C /usr/local --strip-components=1 \
    && rm nodejs.tar.gz \
    && ln -s /usr/local/bin/node /usr/local/bin/nodejs

# Copy csproj files and publish
COPY . .
RUN dotnet publish MiningMonitor.Web/MiningMonitor.Web.csproj -c Release -o /app

# Create final image
FROM runtime
COPY --from=build /app .
ENTRYPOINT ["dotnet", "MiningMonitor.Web.dll"]
