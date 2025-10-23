@echo off
echo ========================================
echo   Moc Chau Fruits AI - Setup Script
echo ========================================
echo.

REM Check Python installation
python --version >nul 2>&1
if errorlevel 1 (
    echo ERROR: Python is not installed or not in PATH
    echo Please install Python 3.8 or higher
    pause
    exit /b 1
)

echo [1/4] Creating virtual environment...
python -m venv venv

echo [2/4] Activating virtual environment...
call venv\Scripts\activate.bat

echo [3/4] Installing dependencies...
pip install --upgrade pip
pip install -r requirements.txt

echo [4/4] Setting up environment file...
if not exist .env (
    copy .env.example .env
    echo Created .env file. Please edit it and add your API keys.
)

echo.
echo ========================================
echo   Setup Complete!
echo ========================================
echo.
echo Next steps:
echo 1. Edit .env file and add your OpenAI API key (optional)
echo 2. Run: python train.py (to load initial data)
echo 3. Run: run.bat (to start the server)
echo.
pause
