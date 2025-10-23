@echo off
echo ========================================
echo Starting Moc Vi Store with AI Chat
echo ========================================
echo.

REM Start AI Server in new window
echo [1/2] Starting AI Server...
start "AI Server" cmd /k "cd Trainning_AI && python app/main.py"

REM Wait for AI server to start
echo Waiting for AI server to initialize...
timeout /t 5 /nobreak > nul

REM Start Web Application
echo [2/2] Starting Web Application...
echo.
echo ========================================
echo Services Starting:
echo - AI Server: http://localhost:8000
echo - Web App: http://localhost:5000
echo ========================================
echo.
dotnet run

pause
