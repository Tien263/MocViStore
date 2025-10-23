# 🚀 Hướng dẫn Deploy API

## Phương án 1: Ngrok (Nhanh nhất - Test/Demo)

### Ưu điểm:
- ✅ Cực kỳ nhanh (5 phút)
- ✅ Không cần config gì
- ✅ Miễn phí

### Nhược điểm:
- ❌ Link thay đổi mỗi lần restart
- ❌ Phải giữ máy tính chạy

### Cách làm:

1. **Download Ngrok**
   - Truy cập: https://ngrok.com/download
   - Tải về và giải nén

2. **Chạy server local**
   ```powershell
   cd C:\Users\ADMIN\Desktop\Trainning_AI
   venv\Scripts\python.exe -m uvicorn app.main:app --host 0.0.0.0 --port 8000
   ```

3. **Mở terminal mới, chạy Ngrok**
   ```powershell
   ngrok http 8000
   ```

4. **Copy link public**
   - Ngrok sẽ hiển thị: `Forwarding https://abc123.ngrok.io -> http://localhost:8000`
   - Gửi link `https://abc123.ngrok.io` cho người khác

5. **API Endpoints**
   - Docs: `https://abc123.ngrok.io/docs`
   - Chat: `https://abc123.ngrok.io/api/chat`

---

## Phương án 2: Railway.app (Khuyến nghị)

### Ưu điểm:
- ✅ Miễn phí (500 giờ/tháng)
- ✅ Deploy tự động từ GitHub
- ✅ Link cố định
- ✅ Không cần giữ máy chạy

### Cách làm:

1. **Push code lên GitHub**
   ```powershell
   git init
   git add .
   git commit -m "Initial commit"
   git remote add origin https://github.com/your-username/your-repo.git
   git push -u origin main
   ```

2. **Deploy trên Railway**
   - Truy cập: https://railway.app
   - Đăng ký/Đăng nhập
   - New Project → Deploy from GitHub
   - Chọn repo của bạn
   - Railway tự động build và deploy

3. **Cấu hình (nếu cần)**
   - Settings → Environment Variables
   - Thêm `OPENAI_API_KEY` nếu có

4. **Nhận link**
   - Railway sẽ cung cấp link: `https://your-app.railway.app`
   - Gửi link này cho người khác

---

## Phương án 3: Render.com

### Ưu điểm:
- ✅ Miễn phí
- ✅ Dễ sử dụng
- ✅ Link cố định

### Nhược điểm:
- ❌ Server sleep sau 15 phút không dùng (khởi động lại mất ~30s)

### Cách làm:

1. **Push code lên GitHub** (như Railway)

2. **Deploy trên Render**
   - Truy cập: https://render.com
   - Đăng ký/Đăng nhập
   - New → Web Service
   - Connect GitHub repo
   - Cấu hình:
     - **Build Command**: `pip install -r requirements.txt && python train.py`
     - **Start Command**: `uvicorn app.main:app --host 0.0.0.0 --port $PORT`
     - **Python Version**: 3.11

3. **Nhận link**
   - `https://your-app.onrender.com`

---

## Phương án 4: Heroku

### Tạo file `Procfile`:
```
web: uvicorn app.main:app --host 0.0.0.0 --port $PORT
```

### Tạo file `runtime.txt`:
```
python-3.11.0
```

### Deploy:
```powershell
heroku login
heroku create your-app-name
git push heroku main
```

---

## Phương án 5: Docker + Cloud Run

### Tạo `Dockerfile`:
```dockerfile
FROM python:3.11-slim

WORKDIR /app

COPY requirements.txt .
RUN pip install --no-cache-dir -r requirements.txt

COPY . .

RUN python train.py

CMD ["uvicorn", "app.main:app", "--host", "0.0.0.0", "--port", "8080"]
```

### Deploy lên Google Cloud Run:
```powershell
gcloud run deploy --source .
```

---

## So sánh các phương án

| Phương án | Miễn phí | Dễ dùng | Link cố định | Tốc độ | Khuyến nghị |
|-----------|----------|---------|--------------|--------|-------------|
| Ngrok | ✅ | ⭐⭐⭐⭐⭐ | ❌ | ⚡⚡⚡ | Test/Demo |
| Railway | ✅ | ⭐⭐⭐⭐ | ✅ | ⚡⚡⚡ | **Tốt nhất** |
| Render | ✅ | ⭐⭐⭐⭐ | ✅ | ⚡⚡ | Backup |
| Heroku | ❌ | ⭐⭐⭐ | ✅ | ⚡⚡ | Trả phí |
| Cloud Run | ✅ | ⭐⭐ | ✅ | ⚡⚡⚡ | Nâng cao |

---

## Sau khi deploy

### Test API:
```bash
# Health check
curl https://your-app.railway.app/api/health

# Chat
curl -X POST https://your-app.railway.app/api/chat \
  -H "Content-Type: application/json" \
  -d '{"question": "Mận có vitamin gì?", "top_k": 3}'
```

### Chia sẻ với người dùng:
- **API Docs**: `https://your-app.railway.app/docs`
- **API Endpoint**: `https://your-app.railway.app/api/chat`

### Tích hợp vào website:
```javascript
const response = await fetch('https://your-app.railway.app/api/chat', {
  method: 'POST',
  headers: { 'Content-Type': 'application/json' },
  body: JSON.stringify({ question: 'Dâu tây có lợi ích gì?', top_k: 3 })
});

const data = await response.json();
console.log(data.answer);
```

---

## Bảo mật (Nếu cần)

Thêm API key authentication vào `app/main.py`:

```python
from fastapi import Header, HTTPException

API_KEY = "your-secret-key"

async def verify_api_key(x_api_key: str = Header(...)):
    if x_api_key != API_KEY:
        raise HTTPException(status_code=401, detail="Invalid API Key")

@app.post("/api/chat", dependencies=[Depends(verify_api_key)])
async def chat(request: QueryRequest):
    # ... existing code
```

Người dùng gọi API:
```bash
curl -X POST https://your-app.railway.app/api/chat \
  -H "Content-Type: application/json" \
  -H "X-API-Key: your-secret-key" \
  -d '{"question": "..."}'
```
