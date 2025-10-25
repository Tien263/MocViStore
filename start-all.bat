@echo off
REM Script khoi dong toan bo he thong (Web + AI Chatbot)
REM Chay script nay de khoi dong ca Web va AI cung luc

echo ========================================
echo   START ALL - Moc Vi Store
echo   Web App + AI Chatbot
echo ========================================
echo.

REM Dung tat ca process dang chay
echo [1/3] Dung tat ca process dang chay...
taskkill /F /IM dotnet.exe >nul 2>&1
taskkill /F /IM Exe_Demo.exe >nul 2>&1
taskkill /F /IM python.exe >nul 2>&1
echo [OK] Da dung tat ca process
echo.

REM Khoi dong AI Chatbot (chay background)
echo [2/3] Khoi dong AI Chatbot...
cd Trainning_AI
start "AI Chatbot Server" cmd /k "set PORT=5000 && python -m app.main"
cd ..
echo [OK] AI Chatbot Server dang khoi dong...
timeout /t 5 /nobreak >nul
echo.

REM Khoi dong Web App
echo [3/3] Khoi dong Web Application...
echo.
echo ========================================
echo   He thong dang chay:
echo   - Web App: http://localhost:5241
echo   - AI Chatbot: http://localhost:5000
echo ========================================
echo.
echo Nhan Ctrl+C de dung Web App
echo De dung AI Chatbot, dong cua so "AI Chatbot"
echo.

dotnet run
