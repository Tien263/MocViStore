@echo off
REM Script Stop ung dung
REM Dung tat ca process lien quan

echo ========================================
echo   STOP - Moc Vi Store
echo ========================================
echo.

echo Dung tat ca process...
taskkill /F /IM dotnet.exe >nul 2>&1
taskkill /F /IM Exe_Demo.exe >nul 2>&1

echo.
echo [OK] Da dung tat ca ung dung!
echo.
pause
