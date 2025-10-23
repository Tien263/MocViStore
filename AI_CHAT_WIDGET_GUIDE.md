# 🤖 Hướng Dẫn Sử Dụng AI Chat Widget

## Tổng Quan

AI Chat Widget là một trợ lý ảo thông minh được tích hợp vào website Mộc Vị Store, giúp khách hàng tìm hiểu về các sản phẩm hoa quả sấy Mộc Châu.

## Tính Năng

✨ **Các tính năng chính:**
- 💬 Chat trực tiếp với AI về sản phẩm hoa quả
- 🎯 Câu hỏi gợi ý nhanh
- 📚 Hiển thị nguồn tham khảo
- 💾 Lưu lịch sử chat
- 📱 Responsive trên mọi thiết bị
- 🎨 Giao diện đẹp mắt, hiện đại

## Cài Đặt

### 1. Khởi động AI Server

Trước tiên, bạn cần khởi động AI server trong folder `Trainning_AI`:

```bash
cd Trainning_AI

# Cài đặt dependencies (lần đầu)
pip install -r requirements.txt

# Khởi động server
python app/main.py
```

Server sẽ chạy tại: `http://localhost:8000`

### 2. Kiểm tra AI Server

Mở trình duyệt và truy cập:
- API Docs: http://localhost:8000/docs
- Health Check: http://localhost:8000/api/health

### 3. Khởi động Website

```bash
# Trong folder chính của dự án
dotnet run
```

Website sẽ chạy tại: `https://localhost:5001` hoặc `http://localhost:5000`

## Cách Sử Dụng

### Cho Người Dùng

1. **Mở Chat Widget**
   - Nhấn vào nút tròn màu xanh ở góc dưới bên phải màn hình
   - Icon: 💬 (biểu tượng chat)

2. **Đặt Câu Hỏi**
   - Gõ câu hỏi vào ô input
   - Hoặc chọn một trong các câu hỏi gợi ý
   - Nhấn Enter hoặc nút gửi

3. **Xem Câu Trả Lời**
   - AI sẽ trả lời dựa trên dữ liệu đã được training
   - Hiển thị nguồn tham khảo bên dưới câu trả lời

4. **Đóng Chat**
   - Nhấn nút X ở góc trên bên phải cửa sổ chat
   - Hoặc nhấn lại nút chat ở góc màn hình

### Ví Dụ Câu Hỏi

```
- "Cho tôi biết về dâu tây sấy"
- "Hoa quả nào tốt cho sức khỏe?"
- "Giá của các sản phẩm như thế nào?"
- "Có những loại hoa quả sấy nào?"
- "Mận sấy có lợi ích gì?"
```

## Cấu Hình

### Thay Đổi API URL

Nếu bạn deploy AI server lên production, cần thay đổi URL trong file `wwwroot/js/ai-chat.js`:

```javascript
class AIChatWidget {
    constructor() {
        // Thay đổi URL này
        this.apiUrl = 'https://your-ai-server.com/api/chat';
        this.healthUrl = 'https://your-ai-server.com/api/health';
        // ...
    }
}
```

### Tùy Chỉnh Giao Diện

Chỉnh sửa file `wwwroot/css/ai-chat.css` để thay đổi:
- Màu sắc
- Kích thước
- Vị trí
- Animation

Ví dụ thay đổi màu chủ đạo:

```css
.ai-chat-button {
    background: linear-gradient(135deg, #82ae46 0%, #5a8a2f 100%);
    /* Thay đổi màu ở đây */
}
```

## Cấu Trúc File

```
Exe_Demo/
├── wwwroot/
│   ├── css/
│   │   └── ai-chat.css          # CSS cho chat widget
│   └── js/
│       └── ai-chat.js            # JavaScript xử lý chat
├── Views/
│   └── Shared/
│       └── _Layout.cshtml        # Đã tích hợp widget
└── Trainning_AI/                 # AI Server
    ├── app/
    │   └── main.py               # FastAPI server
    └── data/
        └── fruits_data.json      # Dữ liệu training
```

## API Endpoints

### POST /api/chat
Chat với AI

**Request:**
```json
{
  "question": "Dâu tây sấy có gì đặc biệt?",
  "top_k": 3
}
```

**Response:**
```json
{
  "answer": "Dâu tây sấy Mộc Châu...",
  "sources": [
    {
      "fruit_name": "Dâu tây",
      "relevance_score": 0.95
    }
  ]
}
```

### GET /api/health
Kiểm tra trạng thái server

**Response:**
```json
{
  "status": "healthy",
  "documents_count": 15
}
```

## Troubleshooting

### Chat Widget không hiển thị

1. Kiểm tra file CSS và JS đã được load:
   - Mở DevTools (F12)
   - Tab Network
   - Tìm `ai-chat.css` và `ai-chat.js`

2. Kiểm tra console có lỗi không

### AI không trả lời

1. **Kiểm tra AI Server có đang chạy không:**
   ```bash
   curl http://localhost:8000/api/health
   ```

2. **Kiểm tra CORS:**
   - Mở DevTools Console
   - Xem có lỗi CORS không
   - AI server đã enable CORS cho tất cả origins

3. **Kiểm tra dữ liệu đã được load:**
   - Truy cập http://localhost:8000/api/health
   - `documents_count` phải > 0

### Lỗi "Đang gặp sự cố kết nối"

1. Đảm bảo AI server đang chạy
2. Kiểm tra URL trong `ai-chat.js` đúng chưa
3. Kiểm tra firewall không block port 8000

## Training AI với Dữ Liệu Mới

### Thêm dữ liệu mới

1. Chỉnh sửa file `Trainning_AI/data/fruits_data.json`
2. Chạy lại training:
   ```bash
   cd Trainning_AI
   python train.py
   ```
3. Khởi động lại server:
   ```bash
   python app/main.py
   ```

### Hoặc thêm qua API

```bash
curl -X POST http://localhost:8000/api/train/add \
  -H "Content-Type: application/json" \
  -d '{
    "id": "new_fruit",
    "fruit_name": "Xoài sấy",
    "description": "...",
    "nutrients": {},
    "health_benefits": [],
    "season": "...",
    "usage": "..."
  }'
```

## Deploy Production

### Deploy AI Server

1. **Railway / Render / Heroku:**
   - Push code lên Git
   - Connect repository
   - Set environment variables
   - Deploy

2. **VPS:**
   ```bash
   # Install dependencies
   pip install -r requirements.txt
   
   # Run with gunicorn
   gunicorn app.main:app -w 4 -k uvicorn.workers.UvicornWorker
   ```

### Cập nhật URL trong Website

Sau khi deploy AI server, cập nhật URL trong `ai-chat.js`:

```javascript
this.apiUrl = 'https://your-production-url.com/api/chat';
this.healthUrl = 'https://your-production-url.com/api/health';
```

## Tính Năng Nâng Cao

### Lưu Lịch Sử Chat

Chat history được lưu trong `localStorage` của trình duyệt:
- Tự động lưu sau mỗi tin nhắn
- Tự động load khi mở lại
- Giới hạn 10 tin nhắn gần nhất

### Typing Indicator

Hiển thị animation "đang gõ..." khi AI đang xử lý câu trả lời.

### Quick Questions

Các câu hỏi gợi ý giúp người dùng bắt đầu cuộc trò chuyện nhanh chóng.

## Support

Nếu gặp vấn đề, hãy:
1. Kiểm tra console log (F12)
2. Kiểm tra AI server logs
3. Đọc lại hướng dẫn này
4. Liên hệ developer

## Credits

- **Framework:** ASP.NET Core MVC
- **AI Backend:** FastAPI + Google Gemini
- **UI Design:** Custom CSS with modern animations
- **Icons:** SVG icons

---

**Phiên bản:** 1.0.0  
**Cập nhật:** 2025-01-23
