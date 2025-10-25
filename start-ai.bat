@echo off
REM Script khoi dong chi AI Chatbot Server

echo ========================================
echo   START AI CHATBOT SERVER
echo ========================================
echo.

echo Dung Python processes...
taskkill /F /IM python.exe >nul 2>&1
echo [OK] Da dung Python
echo.

echo Khoi dong AI Chatbot Server...
cd Trainning_AI
set PORT=5000
python -m app.main
