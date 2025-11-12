# =============================================
# Script gộp tất cả file SQL thành 1 file duy nhất
# =============================================

$outputFile = "MocViStore_COMPLETE_DATABASE.sql"
$encoding = [System.Text.Encoding]::UTF8

# Header
$header = @"
-- =============================================
-- MỘC VỊ STORE - COMPLETE DATABASE SCRIPT
-- Bao gồm: Cấu trúc database + Dữ liệu mẫu đầy đủ
-- Tạo tự động từ: CREATE_FULL_DATABASE.ps1
-- Date: $(Get-Date -Format "yyyy-MM-dd HH:mm:ss")
-- =============================================

USE master;
GO

-- Xóa database cũ nếu tồn tại
IF EXISTS (SELECT name FROM sys.databases WHERE name = 'MocViStoreDB')
BEGIN
    ALTER DATABASE MocViStoreDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE MocViStoreDB;
    PRINT N'✅ Đã xóa database cũ';
END
GO

-- Tạo database mới
CREATE DATABASE MocViStoreDB;
GO

PRINT N'✅ Đã tạo database MocViStoreDB';
GO

USE MocViStoreDB;
GO

PRINT N'';
PRINT N'========================================';
PRINT N'BẮT ĐẦU TẠO DATABASE';
PRINT N'========================================';
GO

"@

# Ghi header
[System.IO.File]::WriteAllText($outputFile, $header, $encoding)

# Danh sách file SQL cần gộp theo thứ tự
$sqlFiles = @(
    "Database\MocViStore_Complete.sql",
    "SQL_Scripts\InsertProductsData.sql",
    "SQL_Scripts\CreateStaffAccount.sql",
    "SQL_Scripts\InsertVouchers.sql",
    "SQL_Scripts\InsertBlogs.sql"
)

Write-Host "Đang gộp các file SQL..." -ForegroundColor Yellow
Write-Host ""

foreach ($file in $sqlFiles) {
    if (Test-Path $file) {
        Write-Host "✅ Đang thêm: $file" -ForegroundColor Green
        
        # Đọc nội dung file
        $content = Get-Content $file -Raw -Encoding UTF8
        
        # Thêm separator
        $separator = @"

-- =============================================
-- FILE: $file
-- =============================================

"@
        
        # Ghi vào file output
        Add-Content -Path $outputFile -Value $separator -Encoding UTF8
        Add-Content -Path $outputFile -Value $content -Encoding UTF8
        
    } else {
        Write-Host "⚠️  Không tìm thấy: $file" -ForegroundColor Yellow
    }
}

# Footer
$footer = @"

-- =============================================
-- HOÀN THÀNH
-- =============================================

PRINT N'';
PRINT N'========================================';
PRINT N'✅ HOÀN THÀNH TẠO DATABASE';
PRINT N'========================================';
PRINT N'';
PRINT N'📊 Thống kê:';
SELECT 'Categories' AS TableName, COUNT(*) AS RecordCount FROM Categories
UNION ALL
SELECT 'Products', COUNT(*) FROM Products
UNION ALL
SELECT 'Employees', COUNT(*) FROM Employees
UNION ALL
SELECT 'Customers', COUNT(*) FROM Customers
UNION ALL
SELECT 'Users', COUNT(*) FROM Users
UNION ALL
SELECT 'Vouchers', COUNT(*) FROM Vouchers
UNION ALL
SELECT 'Blogs', COUNT(*) FROM Blogs;

PRINT N'';
PRINT N'🎯 Tài khoản mặc định:';
PRINT N'   Admin: admin@mocvistore.com / Admin@123';
PRINT N'   Staff: staff@mocvistore.com / Staff@123';
PRINT N'';
PRINT N'⚠️  LƯU Ý: Cần hash password bằng BCrypt trong code C#!';
GO
"@

Add-Content -Path $outputFile -Value $footer -Encoding UTF8

Write-Host ""
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "✅ HOÀN THÀNH!" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "File đã tạo: $outputFile" -ForegroundColor Green
Write-Host "Kích thước: $((Get-Item $outputFile).Length / 1KB) KB" -ForegroundColor Cyan
Write-Host ""
Write-Host "📝 Cách sử dụng:" -ForegroundColor Yellow
Write-Host "   1. Mở SQL Server Management Studio (SSMS)" -ForegroundColor White
Write-Host "   2. Mở file: $outputFile" -ForegroundColor White
Write-Host "   3. Nhấn F5 để chạy" -ForegroundColor White
Write-Host "   4. Database sẽ được tạo tự động với đầy đủ dữ liệu!" -ForegroundColor White
Write-Host ""
