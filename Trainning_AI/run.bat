@echo off
echo ========================================
echo   Moc Chau Fruits AI - Starting Server
echo ========================================
echo.

REM Activate virtual environment if exists
if exist venv\Scripts\activate.bat (
    echo Activating virtual environment...
    call venv\Scripts\activate.bat
)

echo Starting FastAPI server...
echo.
echo Server will be available at: http://localhost:8000
echo Press Ctrl+C to stop the server
echo.

python -m uvicorn app.main:app --reload --host 0.0.0.0 --port 8000
