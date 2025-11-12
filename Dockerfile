# Use .NET 8.0 runtime with Python
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy and restore .NET project
COPY *.csproj ./
RUN dotnet restore

# Copy source and build
COPY . ./
RUN dotnet publish -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Install Python (lightweight)
RUN apt-get update && \
    apt-get install -y --no-install-recommends python3 python3-pip && \
    apt-get clean && \
    rm -rf /var/lib/apt/lists/*

# Copy .NET app
COPY --from=build /app/publish .

# Copy AI service
COPY Trainning_AI ./Trainning_AI

# Install minimal Python deps
WORKDIR /app/Trainning_AI
RUN pip3 install --no-cache-dir --break-system-packages fastapi uvicorn requests google-generativeai pydantic

WORKDIR /app

# Install EF Core tools for migrations
RUN dotnet tool install --global dotnet-ef
ENV PATH="${PATH}:/root/.dotnet/tools"

# Start script with database initialization
COPY <<EOF /app/start.sh
#!/bin/bash
set -e

echo "🔍 Initializing database..."
cd /app

# Apply migrations if needed
if [ ! -f "mocvistore.db" ]; then
    echo "📦 Creating database..."
    dotnet ef database update --no-build || echo "⚠️  Using existing database"
fi

echo "🤖 Starting AI service..."
cd /app/Trainning_AI && python3 -m uvicorn app.main:app --host 0.0.0.0 --port 8000 &

echo "⏳ Waiting for AI service..."
sleep 3

echo "🚀 Starting web application..."
cd /app && dotnet Exe_Demo.dll
EOF

RUN chmod +x /app/start.sh

CMD ["/app/start.sh"]
