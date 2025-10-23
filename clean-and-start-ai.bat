@echo off
echo ========================================
echo CLEAN AND START - Moc Vi Store with AI
echo ========================================
echo.

REM Kill all Python processes
echo [1/5] Stopping all Python processes...
taskkill /F /IM python.exe >nul 2>&1
timeout /t 2 /nobreak >nul

REM Kill dotnet processes
echo [2/5] Stopping dotnet processes...
taskkill /F /IM dotnet.exe >nul 2>&1
timeout /t 2 /nobreak >nul

REM Clean build artifacts
echo [3/5] Cleaning build artifacts...
if exist bin rmdir /s /q bin
if exist obj rmdir /s /q obj
echo Build artifacts cleaned!

REM Start AI Server in new window
echo [4/5] Starting AI Server...
start "AI Server - Moc Vi" cmd /k "cd Trainning_AI && python -m app.main"
echo Waiting for AI server to initialize...
timeout /t 8 /nobreak >nul

REM Start Web Application
echo [5/5] Starting Web Application...
echo.
echo ========================================
echo Services Starting:
echo - AI Server: http://localhost:8000
echo - Web App: http://localhost:5000
echo - Demo Page: http://localhost:5000/ai-chat-demo.html
echo ========================================
echo.
echo Press Ctrl+C to stop all services
echo.

dotnet run

pause
