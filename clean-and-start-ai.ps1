Write-Host "========================================" -ForegroundColor Cyan
Write-Host "CLEAN AND START - Moc Vi Store with AI" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Kill all Python processes
Write-Host "[1/5] Stopping all Python processes..." -ForegroundColor Yellow
Get-Process python -ErrorAction SilentlyContinue | Stop-Process -Force -ErrorAction SilentlyContinue
Start-Sleep -Seconds 2

# Kill dotnet processes
Write-Host "[2/5] Stopping dotnet processes..." -ForegroundColor Yellow
Get-Process dotnet -ErrorAction SilentlyContinue | Stop-Process -Force -ErrorAction SilentlyContinue
Start-Sleep -Seconds 2

# Clean build artifacts
Write-Host "[3/5] Cleaning build artifacts..." -ForegroundColor Yellow
if (Test-Path "bin") {
    Remove-Item -Path "bin" -Recurse -Force
}
if (Test-Path "obj") {
    Remove-Item -Path "obj" -Recurse -Force
}
Write-Host "Build artifacts cleaned!" -ForegroundColor Green

# Start AI Server in new window
Write-Host "[4/5] Starting AI Server..." -ForegroundColor Yellow
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd '$PWD\Trainning_AI'; python -m app.main"
Write-Host "Waiting for AI server to initialize..." -ForegroundColor Yellow
Start-Sleep -Seconds 8

# Start Web Application
Write-Host "[5/5] Starting Web Application..." -ForegroundColor Yellow
Write-Host ""
Write-Host "========================================" -ForegroundColor Green
Write-Host "Services Starting:" -ForegroundColor Green
Write-Host "- AI Server: http://localhost:8000" -ForegroundColor Cyan
Write-Host "- Web App: http://localhost:5000" -ForegroundColor Cyan
Write-Host "- Demo Page: http://localhost:5000/ai-chat-demo.html" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Green
Write-Host ""
Write-Host "Press Ctrl+C to stop all services" -ForegroundColor Yellow
Write-Host ""

dotnet run
