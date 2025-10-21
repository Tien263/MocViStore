# 🔐 Hướng Dẫn Setup Google OAuth 2.0

## ✅ Đã Hoàn Thành

1. ✅ Cài đặt package `Microsoft.AspNetCore.Authentication.Google`
2. ✅ Cấu hình trong `Program.cs`
3. ✅ Thêm `ExternalLogin` và `ExternalLoginCallback` actions
4. ✅ Cập nhật Views với nút Google thực tế

---

## 🔧 Cần Làm: Tạo Google OAuth App

### **Bước 1: Truy cập Google Cloud Console**

1. Vào https://console.cloud.google.com/
2. Đăng nhập bằng tài khoản Google của bạn

### **Bước 2: Tạo Project Mới**

1. Click vào dropdown "Select a project" ở góc trên bên trái
2. Click "NEW PROJECT"
3. Nhập tên project: **Mộc Vị Store**
4. Click "CREATE"

### **Bước 3: Enable Google+ API**

1. Vào menu ☰ → **APIs & Services** → **Library**
2. Tìm "Google+ API"
3. Click vào và nhấn "ENABLE"

### **Bước 4: Tạo OAuth Consent Screen**

1. Vào menu ☰ → **APIs & Services** → **OAuth consent screen**
2. Chọn **External** (cho testing)
3. Click "CREATE"
4. Điền thông tin:
   - **App name**: Mộc Vị Store
   - **User support email**: your-email@gmail.com
   - **Developer contact email**: your-email@gmail.com
5. Click "SAVE AND CONTINUE"
6. **Scopes**: Click "ADD OR REMOVE SCOPES"
   - Chọn: `email`, `profile`, `openid`
   - Click "UPDATE" → "SAVE AND CONTINUE"
7. **Test users**: Click "ADD USERS"
   - Thêm email: your-email@gmail.com
   - Click "ADD" → "SAVE AND CONTINUE"
8. Click "BACK TO DASHBOARD"

### **Bước 5: Tạo OAuth 2.0 Credentials**

**Quan trọng:** Bạn cần vào đúng menu!

1. Ở menu bên trái, tìm và click vào **"API and services"** (có icon 🔌)
2. Trong menu con hiện ra, click **"Credentials"** (Thông tin xác thực)
3. Ở trang Credentials, click nút **"+ CREATE CREDENTIALS"** ở phía trên
4. Chọn **"OAuth client ID"**
5. Nếu chưa configure OAuth consent screen, sẽ yêu cầu làm Bước 4 trước
6. Sau khi đã có consent screen, chọn:
   - **Application type**: **Web application**
   - **Name**: Mộc Vị Store Web
7. **Authorized JavaScript origins**:
   - Click **"+ ADD URI"**
   - Nhập: `http://localhost:5241`
   - Click **"+ ADD URI"** lần nữa
   - Nhập: `https://localhost:7241` (nếu có HTTPS)
8. **Authorized redirect URIs**:
   - Click **"+ ADD URI"**
   - Nhập: `http://localhost:5241/signin-google`
   - Click **"+ ADD URI"** lần nữa
   - Nhập: `https://localhost:7241/signin-google` (nếu có HTTPS)
9. Click **"CREATE"**

### **Bước 6: Copy Client ID và Client Secret**

1. Sau khi tạo, sẽ hiện popup với:
   - **Client ID**: Dạng `123456789-abc...xyz.apps.googleusercontent.com`
   - **Client Secret**: Dạng `GOCSPX-abc...xyz`
2. **COPY cả 2 giá trị này!**

---

## 📝 Cập Nhật appsettings.json

Mở file `appsettings.json` và thay thế:

```json
"Authentication": {
  "Google": {
    "ClientId": "PASTE_CLIENT_ID_HERE",
    "ClientSecret": "PASTE_CLIENT_SECRET_HERE"
  }
}
```

**Ví dụ:**
```json
"Authentication": {
  "Google": {
    "ClientId": "123456789-abcdefghijklmnop.apps.googleusercontent.com",
    "ClientSecret": "GOCSPX-abcdefghijklmnopqrstuvwxyz"
  }
}
```

---

## 🚀 Test Google Login

### **1. Build và chạy app:**
```bash
dotnet clean
dotnet build
dotnet run
```

### **2. Truy cập:**
```
http://localhost:5241/Auth/Login
```

### **3. Click nút "Google"**
- Sẽ redirect đến trang đăng nhập Google
- Chọn tài khoản Google
- Cho phép quyền truy cập
- Redirect về app và tự động đăng nhập!

---

## 🎯 Flow Hoàn Chỉnh

```
1. User click "Đăng nhập với Google"
   ↓
2. Redirect → Google Login Page
   ↓
3. User đăng nhập Google
   ↓
4. Google xác thực
   ↓
5. Redirect → /signin-google (callback)
   ↓
6. System lấy email, name từ Google
   ↓
7. Kiểm tra user đã tồn tại?
   - Nếu CHƯA → Tạo Customer + User mới
   - Nếu RỒI → Lấy user hiện tại
   ↓
8. Tạo claims và đăng nhập
   ↓
9. Redirect → Trang chủ (đã login)
```

---

## ⚠️ Lưu Ý Quan Trọng

### **1. Test Users**
- Trong development, chỉ email được thêm vào "Test users" mới login được
- Để public cho mọi người: Cần publish OAuth consent screen (cần verify domain)

### **2. Redirect URI**
- **PHẢI KHỚP CHÍNH XÁC** với URL trong Google Console
- Bao gồm cả `http://` hoặc `https://`
- Port phải đúng (5241)

### **3. Callback Path**
- Đã set trong `Program.cs`: `/signin-google`
- Không cần tạo action riêng, Google Authentication middleware tự xử lý

### **4. Security**
- **KHÔNG COMMIT** Client Secret vào Git
- Nên dùng User Secrets hoặc Environment Variables trong production

---

## 🔒 Sử Dụng User Secrets (Khuyến Nghị)

Thay vì lưu trong `appsettings.json`, dùng User Secrets:

```bash
dotnet user-secrets init
dotnet user-secrets set "Authentication:Google:ClientId" "YOUR_CLIENT_ID"
dotnet user-secrets set "Authentication:Google:ClientSecret" "YOUR_CLIENT_SECRET"
```

---

## 📧 Email Chào Mừng

- Khi đăng ký qua Google lần đầu → Tự động gửi email chào mừng
- User được tạo với `IsActive = true` (không cần verify OTP)
- Password để trống (vì login qua Google)

---

## ✅ Checklist

- [ ] Tạo Google Cloud Project
- [ ] Enable Google+ API
- [ ] Tạo OAuth Consent Screen
- [ ] Thêm Test Users
- [ ] Tạo OAuth 2.0 Credentials
- [ ] Copy Client ID và Client Secret
- [ ] Cập nhật appsettings.json
- [ ] Build và chạy app
- [ ] Test đăng nhập Google
- [ ] Kiểm tra email chào mừng

---

**Sau khi setup xong, nút "Google" sẽ hoạt động và tự động đăng ký/đăng nhập!** 🎉
