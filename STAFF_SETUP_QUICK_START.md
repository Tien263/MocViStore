# Hướng Dẫn Nhanh - Hệ Thống Quản Lý Staff

## 🚀 Cài Đặt Nhanh

### Bước 1: Tạo Tài Khoản Staff

#### Cách 1: Sử dụng Code (Khuyến nghị)

Thêm code sau vào `Program.cs` (trước `app.Run()`):

```csharp
// Tạo tài khoản Staff mẫu (chỉ chạy 1 lần)
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var helper = new Exe_Demo.Helpers.StaffAccountHelper(context);
    
    try
    {
        var result = await helper.CreateSampleStaffAccountsAsync();
        Console.WriteLine("=== TẠO TÀI KHOẢN STAFF ===");
        Console.WriteLine(result);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Lỗi tạo tài khoản: {ex.Message}");
    }
}
```

**Sau khi chạy 1 lần, hãy xóa đoạn code này để tránh tạo trùng!**

#### Cách 2: Sử dụng SQL Script

Chạy file `SQL_Scripts/CreateStaffAccount.sql` trong SQL Server Management Studio.

**Lưu ý**: Cần cập nhật PasswordHash bằng password đã hash trong code.

### Bước 2: Đăng Nhập

1. Chạy ứng dụng
2. Truy cập `/Auth/Login`
3. Đăng nhập với:
   - **Staff**: `staff@mocvistore.com` / `Staff@123`
   - **Admin**: `admin@mocvistore.com` / `Admin@123`
4. Hệ thống sẽ tự động chuyển đến `/Staff/Dashboard`

## 📋 Tính Năng Chính

### 1. Dashboard (`/Staff/Dashboard`)
- Tổng quan doanh thu hôm nay, tháng này
- Số lượng đơn hàng
- Đơn hàng chờ xử lý
- Cảnh báo sản phẩm sắp hết hàng
- Top sản phẩm bán chạy

### 2. Quản Lý Sản Phẩm (`/Staff/Products`)
- ✅ Xem danh sách sản phẩm (phân trang)
- ✅ Tìm kiếm theo tên/mã
- ✅ Lọc theo danh mục, tồn kho
- ✅ Thêm sản phẩm mới
- ✅ Sửa thông tin sản phẩm
- ✅ Xóa/vô hiệu hóa sản phẩm
- ✅ Cảnh báo tồn kho thấp

### 3. Quản Lý Đơn Hàng (`/Staff/Orders`)
- ✅ Xem danh sách đơn hàng (phân trang)
- ✅ Tìm kiếm theo mã đơn, tên, SĐT
- ✅ Lọc theo trạng thái, thanh toán, ngày
- ✅ Xem chi tiết đơn hàng
- ✅ Cập nhật trạng thái đơn hàng
- ✅ Cập nhật trạng thái thanh toán
- ✅ Thêm ghi chú

### 4. Bán Hàng Trực Tiếp (`/Staff/DirectSale`)
- ✅ Giao diện POS thân thiện
- ✅ Tìm kiếm sản phẩm nhanh
- ✅ Quản lý giỏ hàng
- ✅ Tìm kiếm khách hàng cũ
- ✅ Nhập thông tin khách hàng mới
- ✅ Áp dụng giảm giá
- ✅ Chọn phương thức thanh toán
- ✅ Tự động cập nhật tồn kho

### 5. Thống Kê Doanh Số (`/Staff/SalesReport`)
- ✅ Tổng quan: Doanh thu, lợi nhuận, đơn hàng
- ✅ Biểu đồ doanh thu theo thời gian
- ✅ Top sản phẩm bán chạy
- ✅ Doanh thu theo danh mục (biểu đồ tròn)
- ✅ Phân tích theo phương thức thanh toán
- ✅ Lọc theo khoảng thời gian

## 🔐 Phân Quyền

### Staff
- Truy cập tất cả chức năng quản lý
- Không thể xóa dữ liệu quan trọng (chỉ vô hiệu hóa)

### Admin
- Tất cả quyền của Staff
- Có thể mở rộng thêm quyền quản lý hệ thống

## 📁 Cấu Trúc Files

```
Exe_Demo/
├── Controllers/
│   └── StaffController.cs          # Controller chính
├── Models/
│   └── ViewModels/
│       ├── StaffDashboardViewModel.cs
│       ├── ProductManagementViewModel.cs
│       ├── OrderManagementViewModel.cs
│       ├── DirectSaleViewModel.cs
│       └── SalesReportViewModel.cs
├── Views/
│   └── Staff/
│       ├── Dashboard.cshtml
│       ├── Products.cshtml
│       ├── CreateProduct.cshtml
│       ├── EditProduct.cshtml
│       ├── Orders.cshtml
│       ├── OrderDetail.cshtml
│       ├── DirectSale.cshtml
│       └── SalesReport.cshtml
├── Helpers/
│   └── StaffAccountHelper.cs       # Helper tạo tài khoản
└── SQL_Scripts/
    └── CreateStaffAccount.sql      # Script SQL tạo tài khoản
```

## 🎯 Quy Trình Sử Dụng

### Bán Hàng Trực Tiếp
1. Vào `/Staff/DirectSale`
2. Tìm và chọn sản phẩm
3. Điều chỉnh số lượng trong giỏ
4. Nhập thông tin khách hàng (hoặc tìm khách cũ)
5. Chọn phương thức thanh toán
6. Áp dụng giảm giá (nếu có)
7. Click "Thanh Toán"

### Quản Lý Đơn Hàng
1. Vào `/Staff/Orders`
2. Tìm đơn hàng cần xử lý
3. Click "Xem" để xem chi tiết
4. Cập nhật trạng thái đơn hàng
5. Cập nhật trạng thái thanh toán
6. Thêm ghi chú (nếu cần)
7. Click "Cập Nhật"

### Quản Lý Sản Phẩm
1. Vào `/Staff/Products`
2. Click "Thêm Sản Phẩm Mới"
3. Điền đầy đủ thông tin
4. Click "Lưu Sản Phẩm"

### Xem Thống Kê
1. Vào `/Staff/SalesReport`
2. Chọn khoảng thời gian
3. Chọn loại báo cáo
4. Click "Xem Báo Cáo"

## 🛠️ Troubleshooting

### Không đăng nhập được
- Kiểm tra email và password
- Kiểm tra Role trong database (phải là "Staff" hoặc "Admin")
- Kiểm tra IsActive = true

### Không thấy menu Staff
- Kiểm tra đã đăng nhập với tài khoản Staff chưa
- Kiểm tra EmployeeId có trong Claims không

### Lỗi khi tạo đơn hàng
- Kiểm tra tồn kho sản phẩm
- Kiểm tra thông tin khách hàng đã đầy đủ chưa
- Xem Console log trong browser (F12)

### Biểu đồ không hiển thị
- Kiểm tra đã load Chart.js chưa
- Kiểm tra có dữ liệu trong khoảng thời gian đã chọn không

## 📊 Database Requirements

Các bảng cần thiết:
- ✅ Users (với Role = "Staff" hoặc "Admin")
- ✅ Employees (liên kết với Users)
- ✅ Products
- ✅ Categories
- ✅ Orders
- ✅ OrderDetails
- ✅ Customers

## 🎨 UI/UX Features

- ✅ Responsive design (Bootstrap 5)
- ✅ Font Awesome icons
- ✅ Chart.js cho biểu đồ
- ✅ AJAX cho cập nhật nhanh
- ✅ Toast notifications
- ✅ Loading states
- ✅ Error handling

## 🔄 Workflow

```
Đăng nhập → Dashboard → Chọn chức năng
                ↓
        ┌───────┼───────┬───────────┐
        ↓       ↓       ↓           ↓
    Sản phẩm  Đơn hàng  Bán hàng  Thống kê
```

## 📝 Notes

- Tất cả thao tác đều được log
- Tự động cập nhật tồn kho khi bán hàng
- Không xóa dữ liệu có liên kết (chỉ vô hiệu hóa)
- Session timeout: 2 giờ
- Hỗ trợ Remember Me: 30 ngày

## 🚀 Next Steps

Sau khi setup xong, bạn có thể:
1. Thêm sản phẩm vào hệ thống
2. Tạo đơn hàng test
3. Xem thống kê
4. Tùy chỉnh giao diện theo ý muốn

## 📞 Support

Nếu cần hỗ trợ, kiểm tra:
- `STAFF_SYSTEM_GUIDE.md` - Hướng dẫn chi tiết
- Console logs (F12 trong browser)
- Database logs

---

**Version**: 1.0  
**Created**: 22/10/2025  
**Author**: AI Assistant
