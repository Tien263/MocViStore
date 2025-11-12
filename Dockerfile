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

# Install Python for AI service
RUN apt-get update && \
    apt-get install -y --no-install-recommends python3 python3-pip && \
    apt-get clean && \
    rm -rf /var/lib/apt/lists/*

# Copy .NET app
COPY --from=build /app/publish .

# Copy AI service
COPY Trainning_AI ./Trainning_AI

# Install Python dependencies for AI
WORKDIR /app/Trainning_AI
RUN pip3 install --no-cache-dir --break-system-packages \
    fastapi==0.121.1 \
    uvicorn==0.38.0 \
    google-generativeai==0.8.5 \
    pydantic==2.12.4 \
    requests==2.32.5

WORKDIR /app

# Expose ports
EXPOSE 8080 8000

# Set environment variables
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production
ENV GEMINI_API_KEY=""

# Create startup script
COPY <<EOF /app/start.sh
#!/bin/bash
set -e

echo "🤖 Starting AI service..."
cd /app/Trainning_AI
python3 -m uvicorn app.main:app --host 0.0.0.0 --port 8000 &

echo "⏳ Waiting for AI service to start..."
sleep 5

echo "🚀 Starting web application..."
cd /app
exec dotnet Exe_Demo.dll
EOF

RUN chmod +x /app/start.sh

# Start both services
CMD ["/app/start.sh"]
