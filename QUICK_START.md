# 🚀 Quick Start - Mộc Vị Store

## ⚡ Khởi Động Nhanh (3 Bước)

### Bước 1: Khởi động AI Chatbot Server

**Mở Terminal 1:**
```bash
cd Trainning_AI
python app/main.py
```

Đợi đến khi thấy:
```
INFO:     Uvicorn running on http://0.0.0.0:5000
```

### Bước 2: Khởi động Web App

**Mở Terminal 2 (terminal mới):**
```bash
dotnet run
```

Đợi đến khi thấy:
```
Now listening on: http://localhost:5241
```

### Bước 3: Truy cập

- **Web App:** http://localhost:5241
- **AI Chatbot:** Góc dưới bên phải trang web

---

## 🎯 Hoặc Dùng Scripts

### Chỉ Web App (Không AI)
```bash
.\run.bat
```

### Web App + AI (Thủ công)

**Terminal 1 - AI Server:**
```bash
.\start-ai.bat
```

**Terminal 2 - Web App:**
```bash
.\run.bat
```

---

## 🛑 Dừng Hệ Thống

**Dừng Web App:**
- Nhấn `Ctrl + C` trong terminal Web

**Dừng AI Server:**
- Nhấn `Ctrl + C` trong terminal AI

**Hoặc dừng tất cả:**
```bash
.\stop-all.bat
```

---

## 🔧 Xử Lý Lỗi

### Lỗi: "AI server không kết nối được"

**Kiểm tra AI server có chạy không:**
```bash
curl http://localhost:5000/health
```

**Nếu không chạy, khởi động lại:**
```bash
cd Trainning_AI
python app/main.py
```

### Lỗi: "Port 5241 đã được sử dụng"

```bash
# Dừng tất cả
.\stop-all.bat

# Chạy lại
.\run.bat
```

### Lỗi: "Python không tìm thấy"

**Kiểm tra Python đã cài:**
```bash
python --version
```

**Nếu chưa có, cài Python 3.8+**

### Lỗi: "Module không tìm thấy"

**Cài dependencies:**
```bash
cd Trainning_AI
pip install -r requirements.txt
```

---

## 📋 Checklist Trước Khi Chạy

- [ ] Python 3.8+ đã cài
- [ ] .NET 8.0 SDK đã cài
- [ ] SQL Server đang chạy
- [ ] Database đã tạo
- [ ] File `.env` trong `Trainning_AI` đã cấu hình
- [ ] Dependencies Python đã cài (`pip install -r requirements.txt`)

---

## 🌐 URLs Quan Trọng

| Service | URL | Mô tả |
|---------|-----|-------|
| Web App | http://localhost:5241 | Trang chủ |
| AI API | http://localhost:5000 | API Chatbot |
| AI Health | http://localhost:5000/health | Kiểm tra AI |
| AI Docs | http://localhost:5000/docs | API Documentation |

---

## 💡 Tips

1. **Luôn khởi động AI trước, Web sau**
2. **Kiểm tra AI health trước khi test chatbot**
3. **Dùng 2 terminals riêng biệt cho AI và Web**
4. **Clear browser cache nếu gặp lỗi UI**

---

**Happy Coding! 🎉**
