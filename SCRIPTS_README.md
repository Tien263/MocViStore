# 🚀 Scripts Hướng Dẫn

## 📋 Danh Sách Scripts

### 🌟 Scripts Chạy Toàn Bộ Hệ Thống (Web + AI)

#### 1. `start-all.bat` - Khởi Động Tất Cả
**Mô tả:** Khởi động cả Web App và AI Chatbot cùng lúc

**Khi nào dùng:**
- Chạy hệ thống đầy đủ (Web + AI)
- Phát triển và test chatbot
- Demo đầy đủ tính năng

**Các bước thực hiện:**
1. Dừng tất cả process đang chạy
2. Khởi động AI Chatbot (port 5000)
3. Khởi động Web App (port 5241)

**Cách dùng:**
```bash
.\start-all.bat
```

**Thời gian:** ~10 giây

---

#### 2. `clean-and-start-all.bat` - Clean & Khởi Động Tất Cả
**Mô tả:** Clean, build Web App và khởi động cả Web + AI

**Khi nào dùng:**
- Sau khi thay đổi code lớn
- Gặp lỗi cache
- Build sạch và chạy đầy đủ

**Các bước thực hiện:**
1. Dừng tất cả process
2. Clean Web App
3. Build Web App
4. Khởi động AI Chatbot
5. Khởi động Web App

**Cách dùng:**
```bash
.\clean-and-start-all.bat
```

**Thời gian:** ~30 giây

---

#### 3. `stop-all.bat` - Dừng Tất Cả
**Mô tả:** Dừng cả Web App và AI Chatbot

**Cách dùng:**
```bash
.\stop-all.bat
```

**Thời gian:** ~1 giây

---

### 📦 Scripts Chỉ Web App

#### 4. `clean-and-run.bat` - Clean & Build & Run
**Mô tả:** Clean toàn bộ, build lại và chạy ứng dụng

**Khi nào dùng:**
- Sau khi thay đổi code quan trọng
- Khi gặp lỗi cache
- Khi muốn build sạch từ đầu

**Các bước thực hiện:**
1. Dừng tất cả process đang chạy
2. Clean project (xóa bin, obj)
3. Build lại project
4. Chạy ứng dụng

**Cách dùng:**
```bash
.\clean-and-run.bat
```

**Thời gian:** ~20-30 giây

---

#### 5. `run.bat` - Run Nhanh
**Mô tả:** Chỉ dừng và chạy lại ứng dụng (không clean, không build)

**Khi nào dùng:**
- Chạy lại ứng dụng sau khi dừng
- Thay đổi nhỏ trong Views (Razor)
- Muốn khởi động nhanh

**Các bước thực hiện:**
1. Dừng tất cả process đang chạy
2. Chạy ứng dụng

**Cách dùng:**
```bash
.\run.bat
```

**Thời gian:** ~5-10 giây

---

#### 6. `stop.bat` - Dừng Web App
**Mô tả:** Dừng tất cả process liên quan đến ứng dụng

**Khi nào dùng:**
- Muốn dừng ứng dụng đang chạy
- Trước khi clean hoặc build thủ công
- Khi process bị treo

**Các bước thực hiện:**
1. Dừng tất cả dotnet.exe
2. Dừng tất cả Exe_Demo.exe

**Cách dùng:**
```bash
.\stop.bat
```

**Thời gian:** ~1 giây

---

## 🎯 Workflow Khuyên Dùng

### 🌟 Phát triển với AI Chatbot (Khuyên dùng)
```bash
# Sáng: Khởi động hệ thống đầy đủ
.\start-all.bat

# Thay đổi code nhỏ (Views, CSS)
# → Chỉ cần refresh browser (Ctrl + F5)

# Thay đổi code lớn (Controllers, Models)
# → Dừng Web (Ctrl + C) và chạy lại
.\start-all.bat

# Kết thúc ngày: Dừng tất cả
.\stop-all.bat
```

### 📦 Phát triển chỉ Web (không cần AI)
```bash
# Sáng: Khởi động
.\run.bat

# Thay đổi code nhỏ (Views, CSS)
# → Chỉ cần refresh browser (Ctrl + F5)

# Thay đổi code lớn (Controllers, Models)
# → Dừng (Ctrl + C) và chạy lại
.\run.bat
```

### 🔄 Sau khi pull code mới
```bash
# Với AI:
.\clean-and-start-all.bat

# Chỉ Web:
.\clean-and-run.bat
```

### 🆘 Khi gặp lỗi lạ
```bash
# 1. Dừng tất cả
.\stop-all.bat

# 2. Clean và build lại
.\clean-and-start-all.bat
```

---

## ⚡ So Sánh Scripts

### Scripts Toàn Bộ Hệ Thống (Web + AI)

| Script | Thời gian | Clean | Build | Web | AI | Khi nào dùng |
|--------|-----------|-------|-------|-----|-------|--------------|
| `start-all.bat` | ~10s | ❌ | ❌ | ✅ | ✅ | Chạy nhanh cả hệ thống |
| `clean-and-start-all.bat` | ~30s | ✅ | ✅ | ✅ | ✅ | Build sạch + chạy đầy đủ |
| `stop-all.bat` | ~1s | ❌ | ❌ | ❌ | ❌ | Dừng tất cả |

### Scripts Chỉ Web App

| Script | Thời gian | Clean | Build | Web | Khi nào dùng |
|--------|-----------|-------|-------|-----|--------------|
| `clean-and-run.bat` | ~25s | ✅ | ✅ | ✅ | Thay đổi lớn, lỗi cache |
| `run.bat` | ~8s | ❌ | ❌ | ✅ | Chạy nhanh, thay đổi nhỏ |
| `stop.bat` | ~1s | ❌ | ❌ | ❌ | Chỉ dừng Web |

---

## 🔧 Lệnh Thủ Công (Nếu Cần)

### Clean
```bash
dotnet clean
```

### Build
```bash
dotnet build
```

### Run
```bash
dotnet run
```

### Clean + Build + Run
```bash
dotnet clean && dotnet build && dotnet run
```

---

## 📝 Lưu Ý

1. **Ctrl + C**: Dừng ứng dụng đang chạy trong terminal
2. **Port 5241**: Ứng dụng chạy trên http://localhost:5241
3. **Hot Reload**: ASP.NET Core hỗ trợ hot reload cho Views, không cần restart
4. **Cache Browser**: Nhớ clear cache browser (Ctrl + Shift + R) sau khi thay đổi CSS/JS

---

## 🆘 Xử Lý Lỗi

### Lỗi: "Port 5241 đã được sử dụng"
```bash
# Dừng tất cả process
.\stop.bat

# Hoặc thủ công
taskkill /F /IM dotnet.exe
```

### Lỗi: "Build failed"
```bash
# Clean và build lại
.\clean-and-run.bat
```

### Lỗi: "Process không dừng được"
```bash
# Mở Task Manager (Ctrl + Shift + Esc)
# Tìm và End Task: dotnet.exe, Exe_Demo.exe
```

---

## 🎉 Quick Start

### 🌟 Với AI Chatbot (Khuyên dùng)

**Lần đầu chạy:**
```bash
.\clean-and-start-all.bat
```

**Các lần sau:**
```bash
.\start-all.bat
```

**Dừng hệ thống:**
```bash
.\stop-all.bat
```

### 📦 Chỉ Web App

**Lần đầu chạy:**
```bash
.\clean-and-run.bat
```

**Các lần sau:**
```bash
.\run.bat
```

**Dừng ứng dụng:**
```bash
Ctrl + C (trong terminal)
# hoặc
.\stop.bat
```

---

## 🌐 URLs Hệ Thống

Sau khi chạy `start-all.bat` hoặc `clean-and-start-all.bat`:

- **Web App:** http://localhost:5241
- **AI Chatbot API:** http://localhost:5000
- **AI Chat Widget:** Góc dưới bên phải trang web

---

**Happy Coding! 🚀**
