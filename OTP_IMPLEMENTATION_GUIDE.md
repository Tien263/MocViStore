# 📧 Hướng Dẫn Implement OTP Email Verification

## ✅ Đã Tạo Sẵn

### 1. **Services**
- ✅ `Services/IEmailService.cs` - Interface
- ✅ `Services/EmailService.cs` - Implementation gửi email

### 2. **Models**
- ✅ `Models/OtpVerification.cs` - Model lưu OTP
- ✅ `Models/ViewModels/VerifyOtpViewModel.cs` - ViewModel

### 3. **Database**
- ✅ Đã thêm `DbSet<OtpVerification>` vào ApplicationDbContext
- ⚠️ **CẦN CHẠY MIGRATION:**
  ```bash
  dotnet ef migrations add AddOtpVerification
  dotnet ef database update
  ```

### 4. **Configuration**
- ✅ Đã thêm EmailSettings vào `appsettings.json`
- ⚠️ **CẦN CẬP NHẬT:**
  - SenderEmail: Email Gmail của bạn
  - SenderPassword: App Password (không phải mật khẩu Gmail)

---

## 🔧 Cần Làm Tiếp

### **Bước 1: Đăng Ký EmailService trong Program.cs**

Thêm vào `Program.cs` trước `builder.Build()`:

```csharp
// Email Service
builder.Services.AddScoped<IEmailService, EmailService>();
```

### **Bước 2: Cập Nhật AuthController.Register**

Thay đổi logic đăng ký:

```csharp
[HttpPost]
public async Task<IActionResult> Register(RegisterViewModel model)
{
    if (!ModelState.IsValid)
        return View(model);

    // Kiểm tra email đã tồn tại
    var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
    if (existingUser != null)
    {
        ModelState.AddModelError("Email", "Email đã được sử dụng");
        return View(model);
    }

    // Tạo mã OTP 6 số
    var otpCode = new Random().Next(100000, 999999).ToString();

    // Lưu OTP vào database
    var otpVerification = new OtpVerification
    {
        Email = model.Email,
        OtpCode = otpCode,
        CreatedAt = DateTime.Now,
        ExpiresAt = DateTime.Now.AddMinutes(5)
    };
    _context.OtpVerifications.Add(otpVerification);

    // Lưu thông tin user tạm (chưa active)
    var user = new User
    {
        Email = model.Email,
        FullName = model.FullName,
        PhoneNumber = model.PhoneNumber,
        PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),
        Role = "Customer",
        IsActive = false, // Chưa active
        CreatedDate = DateTime.Now
    };
    _context.Users.Add(user);
    await _context.SaveChangesAsync();

    // Gửi email OTP
    try
    {
        await _emailService.SendOtpEmailAsync(model.Email, otpCode, model.FullName);
        TempData["Email"] = model.Email;
        return RedirectToAction("VerifyOtp");
    }
    catch (Exception ex)
    {
        ModelState.AddModelError("", "Lỗi gửi email: " + ex.Message);
        return View(model);
    }
}
```

### **Bước 3: Tạo Action VerifyOtp**

```csharp
[HttpGet]
public IActionResult VerifyOtp()
{
    var email = TempData["Email"]?.ToString();
    if (string.IsNullOrEmpty(email))
        return RedirectToAction("Register");

    return View(new VerifyOtpViewModel { Email = email });
}

[HttpPost]
public async Task<IActionResult> VerifyOtp(VerifyOtpViewModel model)
{
    if (!ModelState.IsValid)
        return View(model);

    // Tìm OTP
    var otp = await _context.OtpVerifications
        .Where(o => o.Email == model.Email && o.OtpCode == model.OtpCode && !o.IsUsed)
        .OrderByDescending(o => o.CreatedAt)
        .FirstOrDefaultAsync();

    if (otp == null)
    {
        ModelState.AddModelError("OtpCode", "Mã OTP không đúng");
        return View(model);
    }

    if (otp.IsExpired)
    {
        ModelState.AddModelError("OtpCode", "Mã OTP đã hết hạn");
        return View(model);
    }

    // Đánh dấu OTP đã sử dụng
    otp.IsUsed = true;

    // Active user
    var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
    if (user != null)
    {
        user.IsActive = true;
    }

    await _context.SaveChangesAsync();

    // Gửi email chào mừng
    await _emailService.SendWelcomeEmailAsync(model.Email, user.FullName);

    TempData["SuccessMessage"] = "Xác thực thành công! Bạn có thể đăng nhập ngay.";
    return RedirectToAction("Login");
}
```

### **Bước 4: Tạo View VerifyOtp.cshtml**

Tạo file `Views/Auth/VerifyOtp.cshtml` với giao diện đẹp tương tự Login/Register.

---

## 📧 Cấu Hình Gmail

### **Bước 1: Bật 2-Step Verification**
1. Vào https://myaccount.google.com/security
2. Bật "2-Step Verification"

### **Bước 2: Tạo App Password**
1. Vào https://myaccount.google.com/apppasswords
2. Chọn "Mail" và "Windows Computer"
3. Copy mật khẩu 16 ký tự
4. Paste vào `appsettings.json` → `SenderPassword`

### **Bước 3: Cập Nhật appsettings.json**
```json
"EmailSettings": {
  "SenderEmail": "your-real-email@gmail.com",
  "SenderPassword": "abcd efgh ijkl mnop"
}
```

---

## 🎯 Flow Hoàn Chỉnh

1. User điền form đăng ký → Submit
2. System tạo OTP 6 số → Lưu DB
3. System tạo User (IsActive = false)
4. System gửi email OTP
5. Redirect → Trang nhập OTP
6. User nhập OTP → Verify
7. Nếu đúng → Active user → Gửi email chào mừng
8. Redirect → Login

---

## ⚠️ Lưu Ý

- OTP có hiệu lực 5 phút
- Mỗi OTP chỉ dùng 1 lần
- User chưa verify không thể login
- Cần Gmail App Password, không dùng mật khẩu thường

---

## 🚀 Để Test

1. Cập nhật Gmail settings trong appsettings.json
2. Chạy migration
3. Đăng ký tài khoản mới
4. Check email → Nhập OTP
5. Login thành công!
