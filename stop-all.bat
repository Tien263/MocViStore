@echo off
echo ========================================
echo STOPPING ALL SERVICES
echo ========================================
echo.

echo [1/2] Stopping Python (AI Server)...
taskkill /F /IM python.exe >nul 2>&1
if %errorlevel% == 0 (
    echo Python processes stopped!
) else (
    echo No Python processes running.
)

echo [2/2] Stopping dotnet (Web App)...
taskkill /F /IM dotnet.exe >nul 2>&1
if %errorlevel% == 0 (
    echo Dotnet processes stopped!
) else (
    echo No dotnet processes running.
)

echo.
echo ========================================
echo All services stopped!
echo ========================================
pause
