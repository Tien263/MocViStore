# 📖 HƯỚNG DẪN SỬ DỤNG DATABASE

## 🚀 Cách Chạy Database

### **Bước 1: Mở SQL Server Management Studio (SSMS)**

### **Bước 2: Chạy Script**

1. Mở file: `Database/MocViStore_Complete.sql`
2. Nhấn **F5** hoặc click **Execute**
3. Chờ script chạy xong (khoảng 10-20 giây)

### **Bước 3: Kiểm Tra**

```sql
-- Kiểm tra database đã tạo
USE MocViStoreDB;
GO

-- Xem danh sách bảng
SELECT TABLE_NAME 
FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_TYPE = 'BASE TABLE'
ORDER BY TABLE_NAME;

-- Kiểm tra dữ liệu
SELECT * FROM Categories;
SELECT * FROM Products;
SELECT * FROM Employees;
SELECT * FROM Customers;
```

---

## 📊 Dữ Liệu Mẫu Đã Có

### **Categories (4 danh mục)**
- Hoa quả sấy dẻo
- Hoa quả sấy giòn
- Hoa quả sấy thăng hoa
- Combo quà tặng

### **Products (9 sản phẩm)**
- SP001: Mận sấy dẻo Mộc Châu
- SP002: Dâu tây sấy dẻo
- SP003: Kiwi sấy dẻo
- SP004: Xoài sấy giòn
- SP005: Chuối sấy giòn
- SP006: Khoai lang tím sấy giòn
- SP007: Dứa sấy thăng hoa
- SP008: Dâu tây sấy thăng hoa
- SP009: Combo quà tặng Tết

### **Employees (3 nhân viên)**
- NV001: Nguyễn Văn Quản Lý (Manager)
- NV002: Trần Thị Thu Ngân (Cashier)
- NV003: Lê Văn Kho (Warehouse Staff)

### **Users (3 tài khoản)**
- admin@mocvistore.com (Admin)
- cashier@mocvistore.com (Cashier)
- warehouse@mocvistore.com (Staff)

### **Customers (3 khách hàng)**
- KH001: Phạm Văn Khách (VIP - 500 điểm)
- KH002: Hoàng Thị Mua (Thường - 100 điểm)
- KH003: Vũ Văn Thường (Thường - 50 điểm)

### **Suppliers (2 nhà cung cấp)**
- NCC001: Nông trại Mộc Châu
- NCC002: Hợp tác xã Đà Lạt

### **Vouchers (3 mã giảm giá)**
- WELCOME10: Giảm 10% cho khách mới
- TET2024: Giảm 50K dịp Tết
- VIP20: Giảm 20% cho VIP

---

## 🔧 Cấu Hình ASP.NET

### **1. Connection String**

Trong `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=MocViStoreDB;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

### **2. Chạy Migration (Nếu dùng EF)**

```powershell
# Tạo migration
Add-Migration InitialCreate

# Cập nhật database
Update-Database
```

---

## 📝 Các Truy Vấn Hữu Ích

### **1. Xem sản phẩm theo danh mục**
```sql
SELECT p.ProductCode, p.ProductName, c.CategoryName, p.Price, p.StockQuantity
FROM Products p
JOIN Categories c ON p.CategoryId = c.CategoryId
WHERE p.IsActive = 1
ORDER BY c.CategoryName, p.ProductName;
```

### **2. Sản phẩm sắp hết hàng**
```sql
SELECT ProductCode, ProductName, StockQuantity, MinStockLevel
FROM Products
WHERE StockQuantity <= MinStockLevel
ORDER BY StockQuantity;
```

### **3. Top khách hàng VIP**
```sql
SELECT CustomerCode, FullName, PhoneNumber, TotalPurchased, LoyaltyPoints
FROM Customers
WHERE CustomerType = N'VIP'
ORDER BY TotalPurchased DESC;
```

### **4. Nhân viên và chức vụ**
```sql
SELECT e.EmployeeCode, e.FullName, e.Position, e.Department, u.Email, u.Role
FROM Employees e
LEFT JOIN Users u ON e.EmployeeId = u.EmployeeId
WHERE e.IsActive = 1;
```

### **5. Voucher còn hiệu lực**
```sql
SELECT VoucherCode, VoucherName, DiscountType, DiscountValue, 
       MinOrderAmount, ValidFrom, ValidTo, UsageLimit, UsedCount
FROM Vouchers
WHERE IsActive = 1 
  AND GETDATE() BETWEEN ValidFrom AND ValidTo
  AND (UsageLimit IS NULL OR UsedCount < UsageLimit);
```

---

## 🎯 Test Scenarios

### **Scenario 1: Bán hàng online**
```sql
-- 1. Tạo đơn hàng online
INSERT INTO Orders (OrderCode, CustomerId, OrderType, CustomerName, CustomerPhone, TotalAmount, FinalAmount, PaymentMethod, OrderStatus)
VALUES ('DH001', 1, 'Online', N'Phạm Văn Khách', '0911111111', 200000, 200000, N'Chuyển khoản', N'Chờ xác nhận');

-- 2. Thêm chi tiết đơn
INSERT INTO OrderDetails (OrderId, ProductId, ProductName, Price, Quantity, TotalPrice)
VALUES (1, 1, N'Mận sấy dẻo Mộc Châu', 85000, 2, 170000);

-- 3. Cập nhật tồn kho
UPDATE Products SET StockQuantity = StockQuantity - 2 WHERE ProductId = 1;

-- 4. Tạo lịch sử kho
INSERT INTO InventoryTransactions (ProductId, TransactionType, Quantity, ReferenceType, ReferenceId)
VALUES (1, N'Xuất kho', -2, 'Order', 1);
```

### **Scenario 2: Bán hàng trực tiếp (POS)**
```sql
-- 1. Nhân viên mở ca
INSERT INTO Shifts (ShiftCode, EmployeeId, StartTime, OpeningCash, Status)
VALUES ('CA001', 2, GETDATE(), 1000000, N'Đang mở');

-- 2. Tạo đơn POS
INSERT INTO Orders (OrderCode, EmployeeId, OrderType, CustomerName, CustomerPhone, TotalAmount, FinalAmount, PaymentMethod, PaymentStatus, OrderStatus)
VALUES ('POS001', 2, 'POS', N'Khách vãng lai', '0900000000', 85000, 85000, N'Tiền mặt', N'Đã thanh toán', N'Đã hoàn thành');

-- 3. Thanh toán
INSERT INTO Payments (OrderId, PaymentMethod, Amount, EmployeeId, Status)
VALUES (2, N'Tiền mặt', 85000, 2, N'Thành công');
```

### **Scenario 3: Nhập hàng**
```sql
-- 1. Tạo đơn nhập
INSERT INTO PurchaseOrders (PurchaseOrderCode, SupplierId, EmployeeId, TotalAmount, Status, OrderDate)
VALUES ('PN001', 1, 3, 5000000, N'Chờ duyệt', GETDATE());

-- 2. Chi tiết nhập
INSERT INTO PurchaseOrderDetails (PurchaseOrderId, ProductId, Quantity, UnitPrice, TotalPrice)
VALUES (1, 1, 100, 60000, 6000000);

-- 3. Duyệt và nhập kho
UPDATE PurchaseOrders SET Status = N'Đã nhập kho', ReceivedDate = GETDATE() WHERE PurchaseOrderId = 1;
UPDATE Products SET StockQuantity = StockQuantity + 100 WHERE ProductId = 1;
INSERT INTO InventoryTransactions (ProductId, TransactionType, Quantity, ReferenceType, ReferenceId, EmployeeId)
VALUES (1, N'Nhập kho', 100, 'PurchaseOrder', 1, 3);
```

---

## ⚠️ Lưu Ý

1. **Password**: Các password trong Users table cần được hash trước khi sử dụng thực tế
2. **Backup**: Nên backup database thường xuyên
3. **Index**: Database đã có các index cần thiết
4. **Foreign Keys**: Đã cấu hình cascade delete phù hợp
5. **Validation**: Nên thêm validation ở tầng application

---

## 📞 Hỗ Trợ

Nếu gặp lỗi, kiểm tra:
- SQL Server đang chạy
- Quyền truy cập database
- Connection string đúng
- Firewall không chặn SQL Server

---

**Database sẵn sàng sử dụng! 🎉**
