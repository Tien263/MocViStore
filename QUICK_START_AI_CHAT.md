# 🚀 Quick Start - AI Chat Widget

## Khởi Động Nhanh

### Cách 1: Sử dụng Script Tự Động (Khuyên Dùng)

**Windows Command Prompt:**
```bash
start-with-ai.bat
```

**Windows PowerShell:**
```powershell
.\start-with-ai.ps1
```

Script sẽ tự động:
1. ✅ Khởi động AI Server (port 8000)
2. ✅ Khởi động Web Application (port 5000)

### Cách 2: Khởi Động Thủ Công

**Terminal 1 - AI Server:**
```bash
cd Trainning_AI
python app/main.py
```

**Terminal 2 - Web Application:**
```bash
dotnet run
```

## Kiểm Tra

1. **AI Server:** http://localhost:8000/docs
2. **Website:** http://localhost:5000
3. **Chat Widget:** Nhìn góc dưới bên phải màn hình 👉 Nút tròn màu xanh 💬

## Sử Dụng Chat Widget

1. **Nhấn vào nút chat** ở góc dưới bên phải
2. **Gõ câu hỏi** hoặc chọn câu hỏi gợi ý
3. **Nhận câu trả lời** từ AI

### Ví Dụ Câu Hỏi:
- "Dâu tây sấy có gì đặc biệt?"
- "Hoa quả nào tốt cho sức khỏe?"
- "Giá sản phẩm như thế nào?"

## Lưu Ý

⚠️ **Quan trọng:** AI Server phải chạy trước khi sử dụng chat widget!

### Nếu Chat Không Hoạt Động:

1. Kiểm tra AI server đang chạy:
   ```bash
   curl http://localhost:8000/api/health
   ```

2. Xem console log trong DevTools (F12)

3. Đảm bảo có dữ liệu trong `Trainning_AI/data/`

## Tính Năng

✨ **Chat Widget bao gồm:**
- 💬 Chat trực tiếp với AI
- 🎯 Câu hỏi gợi ý nhanh
- 📚 Hiển thị nguồn tham khảo
- 💾 Lưu lịch sử chat
- 📱 Responsive design
- 🎨 Giao diện đẹp, hiện đại

## Xem Thêm

📖 Xem hướng dẫn chi tiết: [AI_CHAT_WIDGET_GUIDE.md](AI_CHAT_WIDGET_GUIDE.md)

---

**Chúc bạn sử dụng vui vẻ! 🎉**
