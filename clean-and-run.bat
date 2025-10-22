@echo off
REM Script Clean va Run ung dung
REM Chay script nay moi khi muon clean va run lai ung dung

echo ========================================
echo   CLEAN ^& RUN - Moc Vi Store
echo ========================================
echo.

REM Buoc 1: Dung process dang chay
echo [1/4] Dung ung dung dang chay...
taskkill /F /IM Exe_Demo.exe >nul 2>&1
if %ERRORLEVEL% EQU 0 (
    echo [OK] Da dung ung dung
) else (
    echo [OK] Khong co ung dung nao dang chay
)
echo.

REM Buoc 2: Clean project
echo [2/4] Clean project...
dotnet clean
if %ERRORLEVEL% EQU 0 (
    echo [OK] Clean thanh cong
) else (
    echo [ERROR] Clean that bai
    pause
    exit /b 1
)
echo.

REM Buoc 3: Build project
echo [3/4] Build project...
dotnet build
if %ERRORLEVEL% EQU 0 (
    echo [OK] Build thanh cong
) else (
    echo [ERROR] Build that bai
    pause
    exit /b 1
)
echo.

REM Buoc 4: Run project
echo [4/4] Chay ung dung...
echo.
echo ========================================
echo   Ung dung dang chay tai:
echo   http://localhost:5241
echo ========================================
echo.
echo Nhan Ctrl+C de dung ung dung
echo.

dotnet run
