# Script Clean và Run ứng dụng
# Chạy script này mỗi khi muốn clean và run lại ứng dụng

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  CLEAN & RUN - Mộc Vị Store" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Bước 1: Dừng process đang chạy
Write-Host "[1/4] Dừng ứng dụng đang chạy..." -ForegroundColor Yellow
try {
    taskkill /F /IM Exe_Demo.exe 2>$null
    Write-Host "✓ Đã dừng ứng dụng" -ForegroundColor Green
} catch {
    Write-Host "✓ Không có ứng dụng nào đang chạy" -ForegroundColor Green
}
Write-Host ""

# Bước 2: Clean project
Write-Host "[2/4] Clean project..." -ForegroundColor Yellow
dotnet clean
if ($LASTEXITCODE -eq 0) {
    Write-Host "✓ Clean thành công" -ForegroundColor Green
} else {
    Write-Host "✗ Clean thất bại" -ForegroundColor Red
    exit 1
}
Write-Host ""

# Bước 3: Build project
Write-Host "[3/4] Build project..." -ForegroundColor Yellow
dotnet build
if ($LASTEXITCODE -eq 0) {
    Write-Host "✓ Build thành công" -ForegroundColor Green
} else {
    Write-Host "✗ Build thất bại" -ForegroundColor Red
    exit 1
}
Write-Host ""

# Bước 4: Run project
Write-Host "[4/4] Chạy ứng dụng..." -ForegroundColor Yellow
Write-Host ""
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  Ứng dụng đang chạy tại:" -ForegroundColor Cyan
Write-Host "  http://localhost:5241" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "Nhấn Ctrl+C để dừng ứng dụng" -ForegroundColor Yellow
Write-Host ""

dotnet run
