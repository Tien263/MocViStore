# 🔧 FIX GOOGLE OAUTH - REDIRECT URI MISMATCH

## ❌ Lỗi hiện tại:
```
Lỗi 400: redirect_uri_mismatch
```

## ✅ CÁCH FIX

### **Bước 1: Lấy URL từ Render**

URL của bạn trên Render (xem trong Dashboard):
```
https://mocvistore-veye.onrender.com
```

---

### **Bước 2: Cập nhật Google Cloud Console**

1. **Vào:** https://console.cloud.google.com/apis/credentials

2. **Chọn OAuth 2.0 Client ID** (đang dùng)

3. **Thêm vào "Authorized JavaScript origins":**
   ```
   https://mocvistore-veye.onrender.com
   ```

4. **Thêm vào "Authorized redirect URIs":**
   ```
   https://mocvistore-veye.onrender.com/signin-google
   ```

5. **Click "Save"**

6. **Đợi 5-10 phút** để Google cập nhật

---

### **Bước 3: Test lại**

1. Vào: https://mocvistore-veye.onrender.com
2. Click "Đăng nhập bằng Google"
3. Chọn tài khoản
4. ✅ Đăng nhập thành công!

---

## 📝 LƯU Ý

**Nếu vẫn lỗi sau 10 phút:**

1. Xóa cookies và cache trình duyệt
2. Thử lại với trình duyệt ẩn danh (Incognito)
3. Kiểm tra lại redirect URI có chính xác không (không có dấu `/` thừa ở cuối)

---

## ✅ CHECKLIST

- [ ] Thêm JavaScript origin: `https://mocvistore-veye.onrender.com`
- [ ] Thêm Redirect URI: `https://mocvistore-veye.onrender.com/signin-google`
- [ ] Click Save
- [ ] Đợi 5-10 phút
- [ ] Test đăng nhập Google
