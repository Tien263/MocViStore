-- =============================================
-- MỘC VỊ STORE - COMPLETE DATABASE SCRIPT
-- Bao gồm: Cấu trúc + Dữ liệu mẫu
-- =============================================

USE master;
GO

-- Tạo database
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'MocViStoreDB')
BEGIN
    CREATE DATABASE MocViStoreDB;
END
GO

USE MocViStoreDB;
GO

-- Import các file SQL khác
-- Chạy theo thứ tự:
-- 1. Database\MocViStore_Complete.sql (Cấu trúc)
-- 2. SQL_Scripts\InsertProductsData.sql (Sản phẩm)
-- 3. SQL_Scripts\CreateStaffAccount.sql (Nhân viên)
-- 4. SQL_Scripts\InsertVouchers.sql (Voucher)
-- 5. SQL_Scripts\InsertBlogs.sql (Blog)

PRINT N'✅ Hướng dẫn: Chạy lần lượt các file SQL trong thư mục Database/ và SQL_Scripts/';
PRINT N'';
PRINT N'Thứ tự chạy:';
PRINT N'1. Database\MocViStore_Complete.sql';
PRINT N'2. SQL_Scripts\InsertProductsData.sql';
PRINT N'3. SQL_Scripts\CreateStaffAccount.sql';
PRINT N'4. SQL_Scripts\InsertVouchers.sql';
PRINT N'5. SQL_Scripts\InsertBlogs.sql';
GO
