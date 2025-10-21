# 🔐 HƯỚNG DẪN SỬ DỤNG HỆ THỐNG ĐĂNG NHẬP/ĐĂNG KÝ

## ✅ Đã Hoàn Thành

### **1. Tạo ViewModels**
- ✅ `LoginViewModel.cs` - Form đăng nhập
- ✅ `RegisterViewModel.cs` - Form đăng ký

### **2. Tạo AuthController**
- ✅ Login (GET/POST) - Xử lý đăng nhập
- ✅ Register (GET/POST) - Xử lý đăng ký
- ✅ Logout (POST) - Đăng xuất
- ✅ Hash password với SHA256
- ✅ Tự động tạo mã khách hàng (KH0001, KH0002...)

### **3. Tạo Views**
- ✅ `Views/Auth/Login.cshtml` - Trang đăng nhập đẹp
- ✅ `Views/Auth/Register.cshtml` - Trang đăng ký đẹp
- ✅ Responsive design
- ✅ Validation messages

### **4. Cấu Hình Authentication**
- ✅ Cookie Authentication
- ✅ Session Management
- ✅ Auto redirect khi chưa đăng nhập

### **5. Cập Nhật Trang Chủ**
- ✅ Nút "Đăng Ký" và "Đăng Nhập" ở header
- ✅ Hiển thị tên user khi đã đăng nhập
- ✅ Nút "Đăng Xuất" khi đã đăng nhập

---

## 🚀 CÁCH SỬ DỤNG

### **Bước 1: Dừng ứng dụng đang chạy**
```powershell
# Nhấn Ctrl+C trong terminal đang chạy
# Hoặc đóng cửa sổ terminal
```

### **Bước 2: Build lại project**
```powershell
dotnet build
```

### **Bước 3: Chạy ứng dụng**
```powershell
dotnet run
```

### **Bước 4: Mở trình duyệt**
```
https://localhost:5001
hoặc
http://localhost:5000
```

---

## 📝 TEST FLOW

### **1. Đăng Ký Tài Khoản Mới**

1. Vào trang chủ
2. Click nút **"Đăng Ký"** ở góc trên bên phải
3. Điền form:
   - Họ tên: `Nguyễn Văn Test`
   - Email: `test@gmail.com`
   - Số điện thoại: `0912345678`
   - Mật khẩu: `123456`
   - Xác nhận mật khẩu: `123456`
   - Địa chỉ: (tùy chọn)
   - Thành phố: (tùy chọn)
4. Click **"Đăng Ký"**
5. Sẽ chuyển về trang Login với thông báo thành công

### **2. Đăng Nhập**

1. Nhập email: `test@gmail.com`
2. Nhập mật khẩu: `123456`
3. (Tùy chọn) Check "Ghi nhớ đăng nhập"
4. Click **"Đăng Nhập"**
5. Sẽ chuyển về trang chủ
6. Ở header sẽ hiển thị: **"Xin chào, Nguyễn Văn Test"**

### **3. Đăng Xuất**

1. Click nút **"Đăng Xuất"** ở header
2. Sẽ logout và quay về trang chủ
3. Header lại hiển thị nút "Đăng Ký" và "Đăng Nhập"

---

## 🎨 Giao Diện

### **Trang Login**
- Background gradient đẹp (tím xanh)
- Form trắng bo tròn với shadow
- Logo Mộc Vị Store
- Icon cho từng field
- Validation realtime
- Link "Quay lại trang chủ"

### **Trang Register**
- Tương tự Login nhưng rộng hơn
- 2 cột cho desktop
- Nhiều fields hơn
- Validation đầy đủ

### **Header Trang Chủ**
- **Chưa đăng nhập**: Hiển thị "Đăng Ký | Đăng Nhập"
- **Đã đăng nhập**: Hiển thị "Xin chào, [Tên] | Đăng Xuất"

---

## 🔒 Bảo Mật

### **Password Hashing**
- Sử dụng SHA256
- Password không lưu dạng plain text
- Hash được lưu trong database

### **Cookie Authentication**
- Secure cookie
- HttpOnly = true
- Expire sau 2 giờ (hoặc 30 ngày nếu check "Ghi nhớ")

### **Session Management**
- Timeout 30 phút
- Auto refresh khi có activity

---

## 📊 Database

### **Khi đăng ký, hệ thống tự động:**

1. **Tạo Customer**
   ```sql
   INSERT INTO Customers (CustomerCode, FullName, PhoneNumber, Email, ...)
   VALUES ('KH0001', 'Nguyễn Văn Test', '0912345678', 'test@gmail.com', ...)
   ```

2. **Tạo User**
   ```sql
   INSERT INTO Users (Email, PasswordHash, FullName, Role, CustomerId, ...)
   VALUES ('test@gmail.com', 'HASHED_PASSWORD', 'Nguyễn Văn Test', 'Customer', 1, ...)
   ```

3. **Liên kết Customer ↔ User**
   - User.CustomerId = Customer.CustomerId
   - Customer có thể có hoặc không có User (khách vãng lai)

---

## 🐛 Troubleshooting

### **Lỗi: "Email đã được sử dụng"**
- Email đã tồn tại trong database
- Dùng email khác hoặc đăng nhập

### **Lỗi: "Email hoặc mật khẩu không đúng"**
- Kiểm tra lại email và password
- Password phân biệt hoa thường

### **Không redirect sau login**
- Kiểm tra authentication đã được cấu hình
- Xem console log có lỗi không

### **Build failed - File locked**
- Dừng ứng dụng đang chạy (Ctrl+C)
- Đóng tất cả terminal
- Build lại

---

## 🎯 Các Tính Năng Đã Có

✅ Đăng ký tài khoản mới
✅ Đăng nhập
✅ Đăng xuất
✅ Ghi nhớ đăng nhập
✅ Validation form đầy đủ
✅ Hash password
✅ Auto tạo mã khách hàng
✅ Tạo Customer + User cùng lúc
✅ Session management
✅ Responsive design
✅ Hiển thị tên user ở header
✅ Redirect về trang chủ sau login

---

## 🔜 Có Thể Mở Rộng

- 🔄 Quên mật khẩu (reset password)
- 📧 Xác thực email
- 👤 Trang profile cá nhân
- 📱 OTP qua SMS
- 🔐 Two-factor authentication
- 📊 Lịch sử đơn hàng
- ⭐ Điểm tích lũy
- 🎁 Voucher của tôi

---

## 📞 Test Accounts

Sau khi chạy `MocViStore_Complete.sql`, bạn có 3 tài khoản:

### **Admin**
- Email: `admin@mocvistore.com`
- Password: (cần hash `Admin@123`)

### **Cashier**
- Email: `cashier@mocvistore.com`
- Password: (cần hash `Cashier@123`)

### **Warehouse**
- Email: `warehouse@mocvistore.com`
- Password: (cần hash `Warehouse@123`)

**Lưu ý:** Passwords trong database hiện tại là placeholder, cần update bằng hash thật.

---

**Hệ thống đăng nhập/đăng ký đã sẵn sàng! 🎉**
