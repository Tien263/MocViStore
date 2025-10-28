@echo off
chcp 65001 >nul
echo ========================================
echo   Deploy Mộc Vị Store to Azure
echo ========================================
echo.

REM Kiểm tra Azure CLI
where az >nul 2>nul
if %ERRORLEVEL% NEQ 0 (
    echo [ERROR] Azure CLI chưa được cài đặt!
    echo Vui lòng cài đặt từ: https://aka.ms/installazurecliwindows
    pause
    exit /b 1
)

echo [1/8] Đăng nhập Azure...
call az login
if %ERRORLEVEL% NEQ 0 (
    echo [ERROR] Đăng nhập thất bại!
    pause
    exit /b 1
)

echo.
echo [2/8] Nhập thông tin cấu hình...
set /p RESOURCE_GROUP="Nhập tên Resource Group (mặc định: MocViStore-RG): "
if "%RESOURCE_GROUP%"=="" set RESOURCE_GROUP=MocViStore-RG

set /p APP_NAME="Nhập tên Web App (phải unique, VD: mocvistore-yourname): "
if "%APP_NAME%"=="" (
    echo [ERROR] Tên Web App không được để trống!
    pause
    exit /b 1
)

set /p LOCATION="Nhập location (mặc định: southeastasia): "
if "%LOCATION%"=="" set LOCATION=southeastasia

echo.
echo [3/8] Tạo Resource Group...
call az group create --name %RESOURCE_GROUP% --location %LOCATION%
echo [OK] Resource Group created

echo.
echo [4/8] Tạo App Service Plan (Free tier)...
call az appservice plan create --name %APP_NAME%-Plan --resource-group %RESOURCE_GROUP% --sku F1 --is-linux
echo [OK] App Service Plan created

echo.
echo [5/8] Tạo Web App...
call az webapp create --resource-group %RESOURCE_GROUP% --plan %APP_NAME%-Plan --name %APP_NAME% --runtime "DOTNET|8.0"
echo [OK] Web App created

echo.
echo [6/8] Build project...
dotnet clean
dotnet publish -c Release -o ./publish
if %ERRORLEVEL% NEQ 0 (
    echo [ERROR] Build thất bại!
    pause
    exit /b 1
)
echo [OK] Build successful

echo.
echo [7/8] Tạo file zip...
powershell Compress-Archive -Path ./publish/* -DestinationPath ./publish.zip -Force
echo [OK] Zip created

echo.
echo [8/8] Deploy lên Azure...
call az webapp deployment source config-zip --resource-group %RESOURCE_GROUP% --name %APP_NAME% --src ./publish.zip
echo [OK] Deploy successful

echo.
echo ========================================
echo   Deploy hoàn tất!
echo ========================================
echo.
echo Website của bạn: https://%APP_NAME%.azurewebsites.net
echo.
echo Các bước tiếp theo:
echo 1. Cấu hình database connection string
echo 2. Cấu hình email settings
echo 3. Cấu hình Google OAuth (nếu cần)
echo.
echo Xem chi tiết trong DEPLOYMENT_GUIDE.md
echo.
pause
