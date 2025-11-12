# Stage 1: Build .NET application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy csproj and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# Stage 2: Runtime with Python for AI service
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Install Python
RUN apt-get update && \
    apt-get install -y python3 python3-pip python3-venv && \
    rm -rf /var/lib/apt/lists/*

# Copy published app
COPY --from=build /app/out .

# Copy AI service
COPY Trainning_AI ./Trainning_AI

# Install Python dependencies
WORKDIR /app/Trainning_AI
RUN pip3 install --no-cache-dir -r requirements.txt

# Back to app directory
WORKDIR /app

# Expose port
EXPOSE 8080

# Start both services
CMD cd Trainning_AI && python3 -m uvicorn app.main:app --host 0.0.0.0 --port 8000 & \
    sleep 5 && \
    dotnet Exe_Demo.dll --urls=http://0.0.0.0:8080
