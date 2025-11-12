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

# Start script
COPY <<EOF /app/start.sh
#!/bin/bash
cd /app/Trainning_AI && python3 -m uvicorn app.main:app --host 0.0.0.0 --port 8000 &
sleep 3
cd /app && dotnet Exe_Demo.dll
EOF

RUN chmod +x /app/start.sh

CMD ["/app/start.sh"]
