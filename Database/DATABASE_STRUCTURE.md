# 📊 Cấu Trúc Database - Mộc Vị Store (Extended)

## Tổng Quan
Database được thiết kế để hỗ trợ **cả bán hàng online và bán hàng trực tiếp tại cửa hàng (POS)**, bao gồm quản lý khách hàng, nhân viên, kho hàng, và tài chính.

---

## 🏗️ Các Nhóm Bảng Chính

### 1️⃣ **QUẢN LÝ SẢN PHẨM**

#### **Categories** - Danh mục sản phẩm
- Phân loại: Sấy dẻo, Sấy giòn, Sấy thăng hoa, Combo quà tặng

#### **Products** - Sản phẩm
- **Mới thêm**: 
  - `ProductCode` - Mã sản phẩm (có thể quét barcode)
  - `CostPrice` - Giá vốn (để tính lợi nhuận)
  - `MinStockLevel` - Mức tồn kho tối thiểu (cảnh báo hết hàng)

---

### 2️⃣ **QUẢN LÝ KHÁCH HÀNG**

#### **Customers** - Khách hàng (Tách riêng khỏi Users)
- Thông tin cá nhân đầy đủ
- Phân loại: Thường, VIP, Đại lý
- Theo dõi:
  - `TotalPurchased` - Tổng tiền đã mua
  - `TotalOrders` - Tổng số đơn hàng
  - `LoyaltyPoints` - Điểm tích lũy
  - `LastPurchaseDate` - Lần mua cuối

#### **LoyaltyPointsHistory** - Lịch sử điểm tích lũy
- Tích điểm khi mua hàng
- Tiêu điểm để đổi quà/giảm giá
- Điều chỉnh điểm thủ công

---

### 3️⃣ **QUẢN LÝ NHÂN VIÊN**

#### **Employees** - Nhân viên
- Thông tin cá nhân
- Chức vụ: Quản lý, Thu ngân, Nhân viên bán hàng, Kho
- Bộ phận: Bán hàng, Kho, Kế toán
- Lương, CMND, Tài khoản ngân hàng

#### **Users** - Tài khoản đăng nhập
- **Liên kết**:
  - `EmployeeId` - Nếu là nhân viên
  - `CustomerId` - Nếu là khách hàng online
- Phân quyền: Admin, Manager, Cashier, Staff, Customer

#### **Shifts** - Ca làm việc
- Quản lý ca làm việc của nhân viên
- Tiền đầu ca, tiền cuối ca
- Tổng doanh thu, tổng đơn hàng trong ca

---

### 4️⃣ **QUẢN LÝ KHO**

#### **Suppliers** - Nhà cung cấp
- Thông tin liên hệ
- Mã số thuế, tài khoản ngân hàng

#### **PurchaseOrders** - Đơn nhập hàng
- Nhập hàng từ nhà cung cấp
- Trạng thái: Chờ duyệt, Đã duyệt, Đã nhập kho, Đã hủy
- Theo dõi công nợ: `PaidAmount`, `RemainingAmount`

#### **PurchaseOrderDetails** - Chi tiết đơn nhập

#### **InventoryTransactions** - Lịch sử xuất nhập kho
- Loại giao dịch: Nhập kho, Xuất kho, Kiểm kê, Hủy hàng
- Liên kết với đơn nhập/đơn bán
- Nhân viên thực hiện

---

### 5️⃣ **QUẢN LÝ ĐƠN HÀNG**

#### **Orders** - Đơn hàng (Cả Online & POS)
- **Mới thêm**:
  - `OrderType` - "Online" hoặc "POS"
  - `CustomerId` - Khách hàng (có thể null nếu khách vãng lai)
  - `EmployeeId` - Nhân viên xử lý (bán trực tiếp)
  - `VoucherCode` - Mã giảm giá
  - `LoyaltyPointsUsed` - Điểm đã dùng
  - `LoyaltyPointsEarned` - Điểm được tích
  - `CompletedDate` - Ngày hoàn thành

#### **OrderDetails** - Chi tiết đơn hàng
- **Mới thêm**: `DiscountPercent` - Giảm giá từng sản phẩm

#### **Payments** - Thanh toán
- Theo dõi chi tiết từng lần thanh toán
- Hỗ trợ thanh toán nhiều lần (trả góp)
- Nhân viên thu tiền (nếu bán trực tiếp)

---

### 6️⃣ **KHUYẾN MÃI & TÍCH ĐIỂM**

#### **Vouchers** - Mã giảm giá
- Loại giảm: Phần trăm hoặc Số tiền cố định
- Điều kiện: Đơn hàng tối thiểu
- Giới hạn số lần sử dụng
- Thời gian hiệu lực

---

### 7️⃣ **TÀI CHÍNH**

#### **Expenses** - Chi phí
- Loại chi phí: Tiền điện, Tiền nước, Lương, Vận chuyển, Khác
- Theo dõi chi phí theo ngày
- Trạng thái: Đã chi, Chờ duyệt

---

### 8️⃣ **WEBSITE & MARKETING**

#### **Cart** - Giỏ hàng
#### **Reviews** - Đánh giá sản phẩm
#### **Blogs** - Bài viết blog
#### **BlogComments** - Bình luận blog
#### **ContactMessages** - Tin nhắn liên hệ
#### **Settings** - Cấu hình website

---

## 📈 Tính Năng Nổi Bật

### ✅ Bán Hàng Trực Tiếp (POS)
- Quản lý ca làm việc
- Nhân viên bán hàng
- Thanh toán tiền mặt/thẻ tại quầy
- In hóa đơn

### ✅ Quản Lý Khách Hàng
- Phân loại khách hàng (Thường, VIP, Đại lý)
- Lịch sử mua hàng
- Điểm tích lũy & đổi quà
- Thống kê khách hàng thân thiết

### ✅ Quản Lý Kho
- Nhập hàng từ nhà cung cấp
- Theo dõi tồn kho realtime
- Cảnh báo hết hàng
- Lịch sử xuất nhập kho
- Kiểm kê định kỳ

### ✅ Báo Cáo & Thống Kê
- Doanh thu theo ngày/tháng/năm
- Doanh thu theo nhân viên
- Sản phẩm bán chạy
- Lợi nhuận (giá bán - giá vốn)
- Công nợ nhà cung cấp
- Chi phí vận hành

### ✅ Khuyến Mãi
- Mã giảm giá
- Giảm giá theo sản phẩm
- Tích điểm đổi quà
- Chương trình khách hàng thân thiết

---

## 🔗 Quan Hệ Chính

```
Customer (1) -----> (N) Orders
Customer (1) -----> (1) User (tài khoản online)
Employee (1) -----> (N) Orders (nhân viên xử lý)
Employee (1) -----> (1) User (tài khoản nội bộ)
Employee (1) -----> (N) Shifts
Product (1) -----> (N) InventoryTransactions
Supplier (1) -----> (N) PurchaseOrders
Order (1) -----> (N) Payments (thanh toán nhiều lần)
```

---

## 📊 Tổng Số Bảng: **24 bảng**

### Bảng mới so với phiên bản cũ:
1. **Customers** - Quản lý khách hàng chi tiết
2. **Employees** - Quản lý nhân viên
3. **Suppliers** - Nhà cung cấp
4. **PurchaseOrders** - Đơn nhập hàng
5. **PurchaseOrderDetails** - Chi tiết đơn nhập
6. **InventoryTransactions** - Lịch sử kho
7. **Payments** - Thanh toán chi tiết
8. **Vouchers** - Mã giảm giá
9. **LoyaltyPointsHistory** - Lịch sử điểm
10. **Shifts** - Ca làm việc
11. **Expenses** - Chi phí

---

## 🚀 Hướng Dẫn Sử Dụng

### Tạo Database:
```sql
-- Chạy file này để tạo database đầy đủ
Database/CreateDatabase_Extended.sql
```

### Sử dụng Entity Framework:
```powershell
# Tạo migration
Add-Migration InitialCreate_Extended

# Cập nhật database
Update-Database
```

---

## 💡 Use Cases

### 1. Bán hàng online:
- Khách hàng đăng ký tài khoản → tạo `Customer` + `User`
- Đặt hàng online → `OrderType = "Online"`
- Tích điểm tự động

### 2. Bán hàng trực tiếp:
- Nhân viên mở ca → tạo `Shift`
- Bán hàng → `OrderType = "POS"`, ghi nhận `EmployeeId`
- Khách vãng lai: `CustomerId = null`
- Khách quen: Quét mã/SĐT → tìm `Customer`
- Đóng ca → tính tổng doanh thu

### 3. Quản lý kho:
- Nhập hàng → tạo `PurchaseOrder`
- Duyệt đơn → tạo `InventoryTransaction` (Nhập kho)
- Bán hàng → tự động tạo `InventoryTransaction` (Xuất kho)
- Kiểm kê → tạo `InventoryTransaction` (Kiểm kê)

---

Database này đủ mạnh để vận hành cả **cửa hàng vật lý** và **website bán hàng online**! 🎉
