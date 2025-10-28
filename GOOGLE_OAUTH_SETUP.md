# 🔐 Hướng Dẫn Cấu Hình Google OAuth

## ⚠️ Lỗi Thường Gặp: redirect_uri_mismatch

### Nguyên nhân:
Google Console chưa được cấu hình đúng URI của website production.

---

## ✅ Giải Pháp: Cấu Hình Google Cloud Console

### Bước 1: Truy cập Google Cloud Console

1. Vào: https://console.cloud.google.com
2. Đăng nhập với tài khoản Google
3. Chọn project của bạn

### Bước 2: Vào Credentials

1. Sidebar → **APIs & Services** → **Credentials**
2. Click vào OAuth 2.0 Client ID của bạn (có tên "Mộc Vị Store" hoặc tương tự)

### Bước 3: Cấu Hình Authorized JavaScript Origins

Trong phần **"Authorized JavaScript origins"**, thêm:

```
https://mocvistore.onrender.com
```

**Lưu ý:**
- ✅ Có `https://` ở đầu
- ✅ KHÔNG có dấu `/` ở cuối
- ✅ Không có port (`:443` hay gì khác)

### Bước 4: Cấu Hình Authorized Redirect URIs

Trong phần **"Authorized redirect URIs"**, thêm:

```
https://mocvistore.onrender.com/signin-google
```

**Lưu ý:**
- ✅ Có `https://` ở đầu
- ✅ Có `/signin-google` ở cuối (chính xác)
- ✅ Không có space hay ký tự thừa

### Bước 5: Giữ Lại URI Local (Để Dev)

Cũng giữ lại các URI local để test ở máy:

**JavaScript origins:**
```
http://localhost:5241
https://localhost:7241
```

**Redirect URIs:**
```
http://localhost:5241/signin-google
https://localhost:7241/signin-google
```

### Bước 6: Save

1. Click **"SAVE"** ở cuối trang
2. Đợi Google xác nhận (vài giây)

---

## 🔍 Kiểm Tra Cấu Hình

### Checklist:

- [ ] Authorized JavaScript origins có: `https://mocvistore.onrender.com`
- [ ] Authorized redirect URIs có: `https://mocvistore.onrender.com/signin-google`
- [ ] Đã click SAVE
- [ ] Đợi 1-2 phút để Google cập nhật
- [ ] Clear browser cache hoặc dùng Incognito

---

## 🔧 Kiểm Tra Render Environment Variables

Vào Render Dashboard → mocvistore → Environment:

Đảm bảo có 2 biến này:

```bash
Authentication__Google__ClientId=your-client-id.apps.googleusercontent.com

Authentication__Google__ClientSecret=your-client-secret
```

**Lấy credentials từ Google Console:**
- Vào: https://console.cloud.google.com
- APIs & Services → Credentials
- Click vào OAuth 2.0 Client ID
- Copy ClientId và ClientSecret

**Lưu ý:** Dùng `__` (2 dấu gạch dưới), KHÔNG dùng `:`

---

## 🧪 Test

### Sau khi cấu hình xong:

1. **Clear browser cache:**
   ```
   Ctrl + Shift + Delete
   → Clear cookies and site data
   ```

2. **Hoặc dùng Incognito:**
   ```
   Ctrl + Shift + N (Chrome)
   ```

3. **Truy cập website:**
   ```
   https://mocvistore.onrender.com
   ```

4. **Click "Đăng nhập" → "Google"**

5. **Nếu vẫn lỗi:**
   - Chụp màn hình lỗi
   - Check Render logs
   - Verify lại Google Console settings

---

## 📸 Screenshot Mẫu

### Google Console - Authorized JavaScript origins:
```
URI 1: http://localhost:5241
URI 2: https://localhost:7241
URI 3: https://mocvistore.onrender.com
```

### Google Console - Authorized redirect URIs:
```
URI 1: http://localhost:5241/signin-google
URI 2: https://localhost:7241/signin-google
URI 3: https://mocvistore.onrender.com/signin-google
```

---

## 🐛 Troubleshooting

### Lỗi: "redirect_uri_mismatch"
**Giải pháp:**
- Kiểm tra lại URI trong Google Console
- Đảm bảo không có space hay ký tự thừa
- Đợi 1-2 phút sau khi save
- Clear browser cache

### Lỗi: "invalid_client"
**Giải pháp:**
- Kiểm tra ClientId và ClientSecret trong Render
- Đảm bảo format đúng: `Authentication__Google__ClientId`

### Lỗi: "access_denied"
**Giải pháp:**
- User từ chối quyền truy cập
- Thử lại với tài khoản khác

---

## 📞 Support

Nếu vẫn gặp vấn đề:
1. Check Render logs: Dashboard → mocvistore → Logs
2. Check Google Console: https://console.cloud.google.com
3. Verify Environment Variables trong Render

---

## ✅ Kết Quả Mong Đợi

Sau khi cấu hình đúng:
- ✅ Click nút "Google" → Chuyển sang trang Google
- ✅ Chọn tài khoản Google
- ✅ Cho phép quyền truy cập
- ✅ Redirect về website và đăng nhập thành công
