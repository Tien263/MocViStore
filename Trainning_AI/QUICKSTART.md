# 🚀 Hướng dẫn nhanh - Quick Start

## Cài đặt và chạy trong 3 bước

### Bước 1: Cài đặt
```bash
setup.bat
```

### Bước 2: Load dữ liệu
```bash
python train.py
```

### Bước 3: Chạy server
```bash
run.bat
```

### Truy cập API
- **Swagger UI**: http://localhost:8000/docs
- **ReDoc**: http://localhost:8000/redoc
- **API Root**: http://localhost:8000

---

## Test API nhanh

### Cách 1: Swagger UI (Dễ nhất)
1. Mở http://localhost:8000/docs
2. Chọn endpoint `/api/chat`
3. Click "Try it out"
4. Nhập câu hỏi và test

### Cách 2: Python
```python
import requests

response = requests.post(
    "http://localhost:8000/api/chat",
    json={"question": "Mận có vitamin gì?", "top_k": 3}
)

print(response.json()['answer'])
```

### Cách 3: cURL
```bash
curl -X POST "http://localhost:8000/api/chat" \
  -H "Content-Type: application/json" \
  -d "{\"question\": \"Hoa quả nào tốt cho tim mạch?\", \"top_k\": 3}"
```

---

## Tùy chọn: OpenAI API

Để có câu trả lời tốt hơn, thêm OpenAI API key vào file `.env`:

```env
OPENAI_API_KEY=sk-your-key-here
```

**Lưu ý**: Không bắt buộc. Hệ thống vẫn hoạt động tốt mà không cần API key.

---

## Thêm dữ liệu của bạn

### Cách 1: Qua API
```python
import requests

requests.post("http://localhost:8000/api/train/add", json={
    "id": "7",
    "fruit_name": "Cam Mộc Châu",
    "description": "Cam ngọt thanh",
    "nutrients": {"vitamin_C": "Cao"},
    "health_benefits": ["Tăng miễn dịch"],
    "season": "Tháng 10-12",
    "usage": "Ăn tươi"
})
```

### Cách 2: Chỉnh sửa file JSON
1. Mở `data/moc_chau_fruits.json`
2. Thêm dữ liệu theo format có sẵn
3. Chạy `python train.py` hoặc gọi API `/api/train/reload`

---

## API Endpoints chính

- `GET /api/health` - Health check
- `POST /api/chat` - Chat với AI
- `GET /api/fruits` - Lấy danh sách hoa quả
- `POST /api/train/add` - Thêm dữ liệu mới
- `POST /api/train/reload` - Reload dữ liệu

---

## Xử lý lỗi

### Lỗi module not found
```bash
venv\Scripts\activate
pip install -r requirements.txt
```

### Lỗi port đã được sử dụng
Đổi port trong file `.env`:
```env
PORT=8001
```

---

**Xem chi tiết trong README.md**
