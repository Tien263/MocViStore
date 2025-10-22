# Hướng Dẫn Lấy TmnCode và HashSecret VNPay

## 🏦 Bước 1: Đăng Ký Tài Khoản VNPay Sandbox

### **Truy cập:**
```
https://sandbox.vnpayment.vn/devreg/
```

### **Điền thông tin đăng ký:**
1. **Tên doanh nghiệp/cá nhân**: Tên của bạn hoặc công ty
2. **Email**: Email để nhận thông tin
3. **Số điện thoại**: Số điện thoại liên hệ
4. **Website**: Website của bạn (có thể để localhost)
5. **Mô tả**: Mô tả ngắn về dự án

### **Gửi đăng ký:**
- Click "Đăng ký"
- Chờ email xác nhận từ VNPay (thường trong vài phút đến vài giờ)

---

## 📧 Bước 2: Nhận Email Xác Nhận

Sau khi đăng ký, bạn sẽ nhận email từ VNPay chứa:

### **Thông tin trong email:**
```
Kính gửi Quý khách,

VNPay xin gửi thông tin tài khoản Sandbox:

- Terminal ID (TmnCode): XXXXXXXX
- Secret Key (HashSecret): XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
- URL thanh toán: https://sandbox.vnpayment.vn/paymentv2/vpcpay.html

Tài liệu API: https://sandbox.vnpayment.vn/apis/docs/
```

---

## 🔑 Bước 3: Lưu Thông Tin

### **Cập nhật vào appsettings.Development.json:**

```json
{
  "VNPay": {
    "TmnCode": "XXXXXXXX",
    "HashSecret": "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX",
    "Url": "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html",
    "ReturnUrl": "http://localhost:5241/Cart/PaymentCallback"
  }
}
```

### **Hoặc cập nhật trực tiếp trong CartController.cs:**

Tìm dòng 319-320:
```csharp
string vnp_TmnCode = "YOUR_TMN_CODE"; // Thay bằng TmnCode từ email
string vnp_HashSecret = "YOUR_HASH_SECRET"; // Thay bằng HashSecret từ email
```

Thay thành:
```csharp
string vnp_TmnCode = "XXXXXXXX"; // TmnCode của bạn
string vnp_HashSecret = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX"; // HashSecret của bạn
```

---

## 🧪 Bước 4: Test Thanh Toán

### **Thông tin thẻ test VNPay Sandbox:**

#### **Thẻ ATM nội địa:**
```
Ngân hàng: NCB
Số thẻ: 9704198526191432198
Tên chủ thẻ: NGUYEN VAN A
Ngày phát hành: 07/15
Mật khẩu OTP: 123456
```

#### **Thẻ quốc tế:**
```
Số thẻ: 4111111111111111
Tên chủ thẻ: NGUYEN VAN A
Ngày hết hạn: 12/25
CVV: 123
```

---

## 📝 Bước 5: Cấu Hình Return URL

### **Trong môi trường Development:**
```
http://localhost:5241/Cart/PaymentCallback
```

### **Trong môi trường Production:**
```
https://yourdomain.com/Cart/PaymentCallback
```

**Lưu ý:** Phải đăng ký Return URL với VNPay trước khi sử dụng!

---

## 🔒 Bảo Mật

### **QUAN TRỌNG:**

1. **KHÔNG commit TmnCode và HashSecret lên GitHub**
2. **Sử dụng appsettings.Development.json** (đã có trong .gitignore)
3. **Trong Production:** Sử dụng Azure Key Vault hoặc Environment Variables

### **Cách sử dụng an toàn:**

**appsettings.json** (commit lên GitHub):
```json
{
  "VNPay": {
    "TmnCode": "YOUR_TMN_CODE",
    "HashSecret": "YOUR_HASH_SECRET",
    "Url": "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html"
  }
}
```

**appsettings.Development.json** (KHÔNG commit):
```json
{
  "VNPay": {
    "TmnCode": "XXXXXXXX",
    "HashSecret": "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX"
  }
}
```

---

## 📚 Tài Liệu Tham Khảo

### **VNPay Documentation:**
- Sandbox: https://sandbox.vnpayment.vn/apis/docs/
- API Reference: https://sandbox.vnpayment.vn/apis/vnpay-api/
- FAQ: https://sandbox.vnpayment.vn/apis/faq/

### **Liên hệ hỗ trợ:**
- Email: support@vnpay.vn
- Hotline: 1900 55 55 77

---

## ⚠️ Lưu Ý Quan Trọng

1. **Sandbox vs Production:**
   - Sandbox: Môi trường test, không giao dịch thật
   - Production: Cần đăng ký doanh nghiệp, có phí giao dịch

2. **Thời gian xử lý:**
   - Đăng ký Sandbox: Vài phút đến vài giờ
   - Đăng ký Production: 3-5 ngày làm việc

3. **Phí giao dịch:**
   - Sandbox: Miễn phí
   - Production: Theo thỏa thuận với VNPay

---

## 🎯 Checklist

- [ ] Đăng ký tài khoản VNPay Sandbox
- [ ] Nhận email chứa TmnCode và HashSecret
- [ ] Cập nhật vào appsettings.Development.json
- [ ] Test thanh toán với thẻ test
- [ ] Kiểm tra callback hoạt động đúng
- [ ] Đảm bảo không commit thông tin nhạy cảm

---

**Chúc bạn tích hợp thành công!** 💳✨
