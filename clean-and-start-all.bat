@echo off
REM Script Clean, Build va khoi dong toan bo he thong
REM Clean Web App, Build lai, roi chay ca Web + AI

echo ========================================
echo   CLEAN ^& START ALL - Moc Vi Store
echo   Clean + Build + Web App + AI Chatbot
echo ========================================
echo.

REM Buoc 1: Dung tat ca process
echo [1/5] Dung tat ca process dang chay...
taskkill /F /IM dotnet.exe >nul 2>&1
taskkill /F /IM Exe_Demo.exe >nul 2>&1
taskkill /F /IM python.exe >nul 2>&1
echo [OK] Da dung tat ca process
echo.

REM Buoc 2: Clean Web App
echo [2/5] Clean Web App...
dotnet clean
if %ERRORLEVEL% EQU 0 (
    echo [OK] Clean thanh cong
) else (
    echo [ERROR] Clean that bai
    pause
    exit /b 1
)
echo.

REM Buoc 3: Build Web App
echo [3/5] Build Web App...
dotnet build
if %ERRORLEVEL% EQU 0 (
    echo [OK] Build thanh cong
) else (
    echo [ERROR] Build that bai
    pause
    exit /b 1
)
echo.

REM Buoc 4: Khoi dong AI Chatbot
echo [4/5] Khoi dong AI Chatbot Server...
cd Trainning_AI
start "AI Chatbot Server" cmd /k "set PORT=5000 && python -m app.main"
cd ..
echo [OK] AI Chatbot Server dang khoi dong...
timeout /t 5 /nobreak >nul
echo.

REM Buoc 5: Khoi dong Web App
echo [5/5] Khoi dong Web Application...
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
