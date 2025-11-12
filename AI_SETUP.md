# 🤖 AI Chat Setup Guide

## 📋 Yêu cầu

Để AI Chat hoạt động, bạn cần **Google Gemini API Key** (miễn phí).

## 🔑 Lấy Gemini API Key

1. **Truy cập**: https://makersuite.google.com/app/apikey
2. **Đăng nhập** với Google account
3. **Click "Create API Key"**
4. **Copy API key** (dạng: `AIzaSy...`)

## ⚙️ Cấu hình cho Render

### **Cách 1: Environment Variables (Khuyên dùng)**

1. **Vào Render Dashboard**: https://dashboard.render.com
2. **Chọn service `mocvistore-3g0e`**
3. **Settings → Environment**
4. **Add Environment Variable**:
   - **Key**: `GEMINI_API_KEY`
   - **Value**: `AIzaSy...` (API key của bạn)
5. **Save Changes**
6. **Manual Deploy** để apply

### **Cách 2: Local Testing**

```bash
# Tạo file .env trong folder Trainning_AI
echo "GEMINI_API_KEY=AIzaSy..." > Trainning_AI/.env
```

## 🚀 Kiểm tra hoạt động

Sau khi deploy:

1. **Truy cập**: https://mocvistore-3g0e.onrender.com
2. **Mở Console** (F12)
3. **Tìm AI chat button** ở góc màn hình
4. **Test chat**: "Xin chào" hoặc "Giới thiệu sản phẩm"

## 📊 Logs để debug

```bash
# Check logs trên Render
- "Starting AI service..." 
- "Su dung Google Gemini 2.0 Flash (mien phi)"
- "Starting web application..."
```

## ❌ Troubleshooting

**Nếu AI không hoạt động:**

1. **Check API Key**: Đúng format và còn hiệu lực
2. **Check Logs**: Có lỗi gì trong startup
3. **Check Console**: Có CORS errors không

**API Key hết quota:**
- Gemini có 1500 requests/day miễn phí
- Tạo API key mới nếu cần
