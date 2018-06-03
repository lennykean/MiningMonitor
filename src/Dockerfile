FROM microsoft/aspnetcore-build:2.0 AS builder
WORKDIR /app

#update npm
RUN npm install -g n
RUN n 8.11.2

# Copy csproj files restore
COPY *.sln ./
COPY MiningMonitor.Web/*.csproj MiningMonitor.Web/
COPY MiningMonitor.BackgroundWorker/*.csproj MiningMonitor.BackgroundWorker/
COPY MiningMonitor.Service/*.csproj MiningMonitor.Service/
COPY MiningMonitor.Data/*.csproj MiningMonitor.Data/
COPY MiningMonitor.Model/*.csproj MiningMonitor.Model/
RUN dotnet restore

# Copy everything else and build
COPY . .
RUN dotnet publish MiningMonitor.Web/MiningMonitor.Web.csproj -c Release -o out

# Build runtime image
FROM microsoft/aspnetcore:2.0

# Install Node.js
RUN apt-get -qq update && apt-get -qqy --no-install-recommends install wget gnupg \
    git \
    unzip
RUN curl -sL https://deb.nodesource.com/setup_6.x |  bash -
RUN apt-get install -y nodejs

WORKDIR /app
COPY --from=builder /app/MiningMonitor.Web/out .
ENTRYPOINT ["dotnet", "MiningMonitor.Web.dll"]
