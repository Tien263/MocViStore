Write-Host "========================================" -ForegroundColor Green
Write-Host "Starting Moc Vi Store with AI Chat" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Green
Write-Host ""

# Start AI Server in new window
Write-Host "[1/2] Starting AI Server..." -ForegroundColor Yellow
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd Trainning_AI; python app/main.py"

# Wait for AI server to start
Write-Host "Waiting for AI server to initialize..." -ForegroundColor Yellow
Start-Sleep -Seconds 5

# Start Web Application
Write-Host "[2/2] Starting Web Application..." -ForegroundColor Yellow
Write-Host ""
Write-Host "========================================" -ForegroundColor Green
Write-Host "Services Starting:" -ForegroundColor Green
Write-Host "- AI Server: http://localhost:8000" -ForegroundColor Cyan
Write-Host "- Web App: http://localhost:5000" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Green
Write-Host ""

dotnet run
