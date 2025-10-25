@echo off
REM Script Run nhanh - Khong clean, chi run
REM Su dung khi chi muon chay lai ung dung

echo ========================================
echo   RUN - Moc Vi Store
echo ========================================
echo.

REM Dung process dang chay
echo [1/2] Dung ung dung dang chay...
taskkill /F /IM dotnet.exe >nul 2>&1
taskkill /F /IM Exe_Demo.exe >nul 2>&1
echo [OK] Da dung tat ca process
echo.

REM Run project
echo [2/2] Chay ung dung...
echo.
echo ========================================
echo   Ung dung dang chay tai:
echo   http://localhost:5241
echo ========================================
echo.
echo Nhan Ctrl+C de dung ung dung
echo.

dotnet run
