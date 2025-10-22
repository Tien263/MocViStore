# 🎯 Hướng Dẫn Cuối Cùng - Chạy Hệ Thống Staff

## ✅ Đã Hoàn Thành

Tôi đã tự động thêm code tạo tài khoản Staff vào `Program.cs`. Bây giờ bạn chỉ cần làm theo các bước sau:

---

## 🚀 Bước 1: Chạy Ứng Dụng

```bash
dotnet run
```

Hoặc nhấn **F5** trong Visual Studio.

---

## 📝 Bước 2: Kiểm Tra Console

Khi ứng dụng khởi động, bạn sẽ thấy trong Console:

```
=== TẠO TÀI KHOẢN STAFF ===
Tạo tài khoản Staff thành công! Mã nhân viên: NV001
Tạo tài khoản Admin thành công! Mã nhân viên: NV002

Tài khoản Staff: staff@mocvistore.com / Staff@123
Tài khoản Admin: admin@mocvistore.com / Admin@123

⚠️  LƯU Ý: Xóa đoạn code tạo tài khoản trong Program.cs sau khi chạy thành công!
```

---

## 🔐 Bước 3: Đăng Nhập

1. Mở browser và truy cập: `https://localhost:xxxx/Auth/Login`
2. Đăng nhập với:
   - **Email**: `staff@mocvistore.com`
   - **Password**: `Staff@123`
3. Hệ thống sẽ tự động chuyển đến Dashboard: `/Staff/Dashboard`

---

## ⚠️ Bước 4: XÓA CODE TẠO TÀI KHOẢN (QUAN TRỌNG!)

Sau khi tạo tài khoản thành công, **BẮT BUỘC** phải xóa đoạn code này trong `Program.cs`:

**Xóa từ dòng 62-82:**
```csharp
// Tạo tài khoản Staff mẫu (chỉ chạy 1 lần khi khởi động)
// XÓA ĐOẠN CODE NÀY SAU KHI ĐÃ TẠO TÀI KHOẢN THÀNH CÔNG!
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var helper = new Exe_Demo.Helpers.StaffAccountHelper(context);
    
    try
    {
        var result = await helper.CreateSampleStaffAccountsAsync();
        Console.WriteLine("\n=== TẠO TÀI KHOẢN STAFF ===");
        Console.WriteLine(result);
        Console.WriteLine("\nTài khoản Staff: staff@mocvistore.com / Staff@123");
        Console.WriteLine("Tài khoản Admin: admin@mocvistore.com / Admin@123");
        Console.WriteLine("\n⚠️  LƯU Ý: Xóa đoạn code tạo tài khoản trong Program.cs sau khi chạy thành công!\n");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Lỗi tạo tài khoản Staff: {ex.Message}");
    }
}
```

**Lý do**: Nếu không xóa, mỗi lần khởi động sẽ cố gắng tạo lại tài khoản và gây lỗi.

---

## 🎉 Bước 5: Khám Phá Hệ Thống

Sau khi đăng nhập, bạn có thể truy cập:

### 📊 Dashboard
- URL: `/Staff/Dashboard`
- Xem tổng quan doanh thu, đơn hàng, sản phẩm bán chạy

### 📦 Quản Lý Sản Phẩm
- URL: `/Staff/Products`
- Thêm/sửa/xóa sản phẩm
- Tìm kiếm và lọc

### 📋 Quản Lý Đơn Hàng
- URL: `/Staff/Orders`
- Xem danh sách đơn hàng
- Cập nhật trạng thái

### 💰 Bán Hàng Trực Tiếp
- URL: `/Staff/DirectSale`
- Giao diện POS
- Tạo đơn hàng tại quầy

### 📈 Thống Kê Doanh Số
- URL: `/Staff/SalesReport`
- Biểu đồ doanh thu
- Báo cáo chi tiết

---

## 🔧 Nếu Gặp Lỗi

### Lỗi: "Email đã tồn tại"
➡️ Tài khoản đã được tạo trước đó. Bỏ qua và đăng nhập bình thường.

### Lỗi: "Connection string"
➡️ Kiểm tra `appsettings.json` có connection string đúng chưa.

### Lỗi: "Table not found"
➡️ Chạy migration: `dotnet ef database update`

### Không redirect đến Dashboard
➡️ Kiểm tra Role trong database phải là "Staff" hoặc "Admin"

---

## 📚 Tài Liệu Tham Khảo

- **STAFF_SETUP_QUICK_START.md** - Hướng dẫn nhanh
- **STAFF_SYSTEM_GUIDE.md** - Hướng dẫn chi tiết
- **STAFF_IMPLEMENTATION_SUMMARY.md** - Tóm tắt kỹ thuật

---

## ✨ Tính Năng Đã Có

✅ Dashboard tổng quan  
✅ CRUD sản phẩm  
✅ Quản lý đơn hàng  
✅ Bán hàng trực tiếp (POS)  
✅ Thống kê doanh số với biểu đồ  
✅ Tìm kiếm & lọc mạnh mẽ  
✅ Responsive design  
✅ AJAX realtime updates  

---

## 🎯 Checklist

- [ ] Chạy ứng dụng lần đầu
- [ ] Kiểm tra Console thấy thông báo tạo tài khoản
- [ ] Đăng nhập với staff@mocvistore.com
- [ ] Truy cập Dashboard thành công
- [ ] **XÓA code tạo tài khoản trong Program.cs**
- [ ] Test các chức năng chính
- [ ] Đọc tài liệu hướng dẫn

---

## 🎊 Hoàn Tất!

Hệ thống Staff đã sẵn sàng sử dụng. Chúc bạn làm việc hiệu quả! 🚀

---

**Lưu ý cuối**: Nhớ xóa code tạo tài khoản sau khi chạy thành công lần đầu!
