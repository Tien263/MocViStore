# 🍓 Mộc Vị AI - Sales Consultant API

**AI Tư Vấn Bán Hàng Chuyên Nghiệp** cho thương hiệu hoa quả sấy cao cấp **Mộc Vị** từ Mộc Châu. Sử dụng công nghệ **RAG (Retrieval-Augmented Generation)** + **Gemini AI** để tư vấn sản phẩm, giá cả, lợi ích sức khỏe với phong cách nhiệt tình, chuyên nghiệp như sales consultant thực thụ.

## ✨ Tính năng Đặc Biệt

- 🎯 **AI Sales Consultant**: Tư vấn bán hàng chuyên nghiệp, nhiệt tình, lôi cuốn
- 💬 **Streaming Response**: Trả lời từng chữ một, mượt mà như ChatGPT
- 💰 **Tư vấn giá cả**: Trả lời chính xác về giá, khuyến mãi, combo
- 🔍 **RAG System**: Tìm kiếm thông tin sản phẩm chính xác từ vector database
- 📊 **Multi-Source Data**: Hỗ trợ nhiều file JSON (sản phẩm, thương hiệu, mùa vụ, bảo quản)
- 🌐 **RESTful API**: Tích hợp dễ dàng vào website, app, chatbot
- 📚 **Auto Documentation**: Swagger UI tích hợp sẵn

## 🏗️ Kiến trúc

```
┌─────────────────┐      ┌──────────────┐      ┌─────────────┐
│  Client App     │ ───> │  FastAPI     │ ───> │  Vector DB  │
│  (Web/Mobile)   │ <─── │  REST API    │ <─── │  (ChromaDB) │
└─────────────────┘      └──────────────┘      └─────────────┘
                                │
                                ▼
                         ┌──────────────┐
                         │  Gemini AI   │
                         │  (Streaming) │
                         └──────────────┘
```

## 📋 Yêu cầu hệ thống

- Python 3.11 trở lên
- 2GB RAM trở lên
- Kết nối internet (để download models lần đầu)
- **Google Gemini API key** (miễn phí) - Bắt buộc để AI trả lời

## 🚀 Cài đặt nhanh

### Bước 1: Clone hoặc download dự án

```bash
cd Trainning_AI
```

### Bước 2: Chạy script setup (Windows)

```bash
setup.bat
```

Script sẽ tự động:
- Tạo virtual environment
- Cài đặt các dependencies
- Tạo file `.env`

### Bước 3: Cấu hình API Key

Mở file `.env` và thêm **Gemini API key**:

```env
GEMINI_API_KEY=your-gemini-api-key-here
```

**Lấy Gemini API key miễn phí:**
1. Truy cập: https://makersuite.google.com/app/apikey
2. Click "Create API Key"
3. Copy và paste vào file `.env`

**Lưu ý**: Gemini API miễn phí với 60 requests/phút, đủ cho hầu hết use cases.

### Bước 4: Load dữ liệu

```bash
python train.py
```

### Bước 5: Chạy server

```bash
run.bat
```

Hoặc:

```bash
python -m uvicorn app.main:app --reload
```

### Bước 6: Truy cập API

- **API Documentation (Swagger)**: http://localhost:8000/docs
- **API Documentation (ReDoc)**: http://localhost:8000/redoc
- **API Root**: http://localhost:8000

## 📁 Cấu trúc dự án

```
Trainning_AI/
├── app/
│   ├── __init__.py
│   ├── main.py              # FastAPI application
│   ├── config.py            # Cấu hình
│   ├── simple_vector_store.py  # Vector database (Simple)
│   └── llm_service.py       # Gemini AI service với streaming
├── data/
│   ├── moc_chau_fruits.json    # Dữ liệu sản phẩm (3 sản phẩm mẫu)
│   ├── brand_info.json         # Thông tin thương hiệu Mộc Vị
│   ├── seasonal_calendar.json  # Lịch mùa vụ 12 tháng
│   └── storage_guide.json      # Hướng dẫn bảo quản chi tiết
├── simple_vector_db.pkl     # Vector database (tự động tạo)
├── requirements.txt         # Python dependencies
├── .env.example             # Mẫu file cấu hình
├── train.py                 # Script training
├── chat.py                  # Script chat local (test)
├── test_gemini.py           # Test Gemini API
├── setup.bat                # Script cài đặt
├── run.bat                  # Script chạy server
└── README.md                # File này
```

## 📝 Cách sử dụng API

### 1. Sử dụng Swagger UI (Khuyến nghị cho người mới)

Truy cập http://localhost:8000/docs để:
- Xem tất cả endpoints
- Test API trực tiếp trên trình duyệt
- Xem request/response schema

### 2. Sử dụng Python

```python
import requests

# Chat với AI Sales Consultant
response = requests.post(
    "http://localhost:8000/api/chat",
    json={
        "question": "50.000đ mua được gì?",
        "top_k": 3
    }
)

data = response.json()
print(data['answer'])
# AI sẽ trả lời: "Với 50.000đ bạn có thể mua 2 gói mini 50g dâu tây sấy dẻo (25.000đ/gói)..."
```

### 3. Sử dụng cURL

```bash
curl -X POST "http://localhost:8000/api/chat" \
  -H "Content-Type: application/json" \
  -d '{"question": "Hoa quả nào tốt cho tim mạch?", "top_k": 3}'
```

### 4. Sử dụng JavaScript/Fetch

```javascript
const response = await fetch('http://localhost:8000/api/chat', {
  method: 'POST',
  headers: { 'Content-Type': 'application/json' },
  body: JSON.stringify({
    question: 'Dâu tây có lợi ích gì?',
    top_k: 3
  })
});

const data = await response.json();
console.log(data.answer);
```

## 🔌 API Endpoints

### GET `/`
Thông tin về API

### GET `/api/health`
Health check và số lượng documents

### POST `/api/chat`
Chat với AI

**Request:**
```json
{
  "question": "string",
  "top_k": 3  // optional
}
```

**Response:**
```json
{
  "answer": "string",
  "sources": [
    {
      "fruit_name": "string",
      "relevance_score": 0.95
    }
  ]
}
```

### GET `/api/fruits`
Lấy danh sách tất cả hoa quả

### POST `/api/train/add`
Thêm dữ liệu hoa quả mới

**Request:**
```json
{
  "id": "string",
  "fruit_name": "string",
  "description": "string",
  "nutrients": {},
  "health_benefits": [],
  "season": "string",
  "usage": "string"
}
```

### POST `/api/train/reload`
Reload dữ liệu từ file JSON

## 📊 Quản lý dữ liệu

### Cách 1: Qua API

```python
import requests

# Thêm hoa quả mới
requests.post("http://localhost:8000/api/train/add", json={
    "id": "7",
    "fruit_name": "Cam Mộc Châu",
    "description": "Cam ngọt thanh",
    "nutrients": {"vitamin_C": "Cao"},
    "health_benefits": ["Tăng miễn dịch"],
    "season": "Tháng 10-12",
    "usage": "Ăn tươi"
})

# Reload dữ liệu
requests.post("http://localhost:8000/api/train/reload")
```

### Cách 2: Chỉnh sửa file JSON
1. Mở `data/moc_chau_fruits.json`
2. Thêm object mới theo format:

```json
{
  "id": "7",
  "fruit_name": "Tên hoa quả",
  "description": "Mô tả...",
  "nutrients": {
    "vitamin_C": "Mô tả...",
    "kali": "Mô tả..."
  },
  "health_benefits": [
    "Lợi ích 1",
    "Lợi ích 2"
  ],
  "season": "Tháng 1-3",
  "usage": "Ăn tươi, làm mứt"
}
```

3. Chạy lại `python train.py` hoặc gọi API `/api/train/reload`

## 🧪 Test AI Local (Không cần API)

### Test nhanh với chat.py

```bash
python chat.py
```

**Thử các câu hỏi:**
```
💬 Bạn: Dâu tây có tốt cho sức khỏe không?
💬 Bạn: 50.000đ mua được gì?
💬 Bạn: Giá dâu sấy thăng hoa bao nhiêu?
💬 Bạn: Tôi muốn mua quà tặng cao cấp
💬 Bạn: Mận có giúp tiêu hóa không?
💬 Bạn: Tháng 7 có hoa quả gì?
💬 Bạn: Cách bảo quản hoa quả sấy
```

**AI sẽ trả lời với phong cách:**
- ✅ Nhiệt tình, chuyên nghiệp như sales consultant
- ✅ Trả lời chính xác về giá, khuyến mãi
- ✅ Highlight lợi ích, điểm mạnh sản phẩm
- ✅ Streaming response (từng chữ một)
- ✅ Gợi ý mua hàng tinh tế

## 🧪 Test API

### Cách 1: Chạy script test

```bash
python test_api.py
```

### Cách 2: Sử dụng REST Client (VS Code)

1. Cài extension "REST Client" trong VS Code
2. Mở file `api_examples.http`
3. Click "Send Request" trên mỗi endpoint

### Cách 3: Sử dụng Postman

Import các endpoint từ Swagger UI hoặc tạo collection mới với các endpoint trên.

## 🔧 Cấu hình nâng cao

Chỉnh sửa file `app/config.py`:

```python
# Model cấu hình
EMBEDDING_MODEL = "sentence-transformers/paraphrase-multilingual-MiniLM-L12-v2"
LLM_MODEL = "gpt-3.5-turbo"

# RAG cấu hình
CHUNK_SIZE = 500
CHUNK_OVERLAP = 50
TOP_K_RESULTS = 3  # Số lượng kết quả tìm kiếm
```

## 🌟 Tích hợp vào ứng dụng

### Tích hợp vào Website

```html
<script>
async function askFruitAI(question) {
  const response = await fetch('http://localhost:8000/api/chat', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ question, top_k: 3 })
  });
  
  const data = await response.json();
  return data.answer;
}

// Sử dụng
const answer = await askFruitAI('Mận có vitamin gì?');
console.log(answer);
</script>
```

### Tích hợp vào React

```jsx
import { useState } from 'react';

function FruitChatbot() {
  const [question, setQuestion] = useState('');
  const [answer, setAnswer] = useState('');

  const askAI = async () => {
    const response = await fetch('http://localhost:8000/api/chat', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ question, top_k: 3 })
    });
    const data = await response.json();
    setAnswer(data.answer);
  };

  return (
    <div>
      <input value={question} onChange={(e) => setQuestion(e.target.value)} />
      <button onClick={askAI}>Hỏi AI</button>
      <p>{answer}</p>
    </div>
  );
}
```

### Tích hợp vào Mobile App (React Native)

```javascript
const askAI = async (question) => {
  try {
    const response = await fetch('http://your-server:8000/api/chat', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ question, top_k: 3 })
    });
    const data = await response.json();
    return data.answer;
  } catch (error) {
    console.error('Error:', error);
  }
};
```

## 🔐 Bảo mật

- **API Key**: Không commit file `.env` vào git
- **CORS**: Cấu hình CORS trong `app/main.py` cho production
- **Rate Limiting**: Thêm rate limiting cho API endpoints
- **Authentication**: Thêm authentication nếu cần

## 🐛 Xử lý lỗi thường gặp

### Lỗi: "Module not found"
```bash
# Kích hoạt virtual environment
venv\Scripts\activate
# Cài lại dependencies
pip install -r requirements.txt
```

### Lỗi: "Port 8000 already in use"
```bash
# Thay đổi port trong .env
PORT=8001
```

### Lỗi: "OpenAI API error"
- Kiểm tra API key trong file `.env`
- Hoặc để trống để sử dụng chế độ không cần API key

## 📊 Performance

- **Tốc độ tìm kiếm**: < 100ms
- **Tốc độ response (với OpenAI)**: 1-3s
- **Tốc độ response (không OpenAI)**: < 200ms
- **Số lượng documents**: Hỗ trợ hàng nghìn documents

## 🔄 Cập nhật

Để cập nhật dữ liệu:

1. Chỉnh sửa `data/moc_chau_fruits.json`
2. Chạy `python train.py` hoặc click "Reload dữ liệu" trong giao diện

## 📚 Công nghệ sử dụng

- **Backend**: FastAPI, Python 3.11
- **Vector DB**: SimpleVectorStore (pickle-based) hoặc ChromaDB
- **Embeddings**: Sentence Transformers (all-MiniLM-L6-v2)
- **LLM**: Google Gemini 2.0 Flash (miễn phí, streaming)
- **Frontend**: HTML, TailwindCSS, JavaScript

## 🎯 Về Mộc Vị

**Mộc Vị** là thương hiệu hoa quả sấy cao cấp từ Mộc Châu, Sơn La.

- **Mộc**: Mộc mạc, tự nhiên, nguyên bản từ núi rừng Tây Bắc
- **Vị**: Hương vị, trải nghiệm khi thưởng thức

**Sản phẩm hiện tại:**
- 🍓 Dâu Tây Sấy Dẻo (90.000đ/200g)
- 💎 Dâu Tây Sấy Thăng Hoa PREMIUM (140.000đ/100g)
- 🍑 Mận Sấy Dẻo (65.000đ/200g)
- *(Và nhiều sản phẩm khác đang được bổ sung)*

**Giá trị cốt lõi:**
- Giữ trọn tự nhiên - 100% từ Mộc Châu
- Tôn vinh bản sắc - Câu chuyện núi rừng Tây Bắc
- Cam kết chất lượng - Minh bạch, an toàn
- Bao bì xanh - Giấy phân hủy sinh học

## 🤝 Đóng góp

Mọi đóng góp đều được chào đón! Hãy tạo issue hoặc pull request.

## 📄 License

MIT License - Tự do sử dụng cho mục đích cá nhân và thương mại.

## 💡 Ý tưởng mở rộng

- [ ] Thêm hỗ trợ upload file (PDF, Word) để training
- [ ] Tích hợp voice input/output
- [ ] Multi-language support
- [ ] Analytics dashboard
- [ ] Mobile app
- [ ] Chatbot widget cho website

## 🌐 Deploy MIỄN PHÍ - Có link public cho mọi người

### **🏆 Option 1: Render.com (Khuyến nghị nhất - Free Forever)**

**Ưu điểm:**
- ✅ **Miễn phí vĩnh viễn** - Không giới hạn thời gian
- ✅ 750 giờ/tháng (đủ chạy 24/7)
- ✅ Auto-deploy từ GitHub
- ✅ HTTPS miễn phí
- ✅ Không cần credit card

**Cách deploy:**

1. **Truy cập**: https://render.com
2. **Sign up** bằng GitHub
3. Click **"New +"** → **"Web Service"**
4. Chọn repo `moc-chau-ai-api`
5. Cấu hình:
   - **Name**: `moc-vi-ai`
   - **Environment**: `Python 3`
   - **Build Command**: `pip install -r requirements.txt && python train.py`
   - **Start Command**: `uvicorn app.main:app --host 0.0.0.0 --port $PORT`
   - **Plan**: **Free**
6. Thêm **Environment Variables**:
   ```
   GEMINI_API_KEY=your-gemini-api-key-here
   ```
7. Click **"Create Web Service"**

**Link của bạn:** `https://moc-vi-ai.onrender.com`

**Lưu ý:** 
- Lần đầu deploy mất ~5-10 phút
- App sleep sau 15 phút không dùng, request đầu tiên sẽ mất ~30s để wake up
- Hoàn toàn đủ cho demo và test

---

### **Option 2: Hugging Face Spaces (Miễn phí, không giới hạn)**

**Ưu điểm:**
- ✅ Miễn phí vĩnh viễn
- ✅ Không sleep
- ✅ Chạy 24/7
- ✅ Hỗ trợ Gradio UI (giao diện chat đẹp)

**Cách deploy:**

1. Truy cập: https://huggingface.co/spaces
2. Click **"Create new Space"**
3. Chọn **"Gradio"** SDK
4. Upload code hoặc link GitHub
5. Thêm `GEMINI_API_KEY` vào Secrets

**Link:** `https://huggingface.co/spaces/your-username/moc-vi-ai`

---

### **Option 3: Google Cloud Run (Miễn phí 2 triệu requests/tháng)**

**Ưu điểm:**
- ✅ 2 triệu requests miễn phí/tháng
- ✅ Không sleep
- ✅ Tốc độ nhanh (Google infrastructure)
- ✅ Scale tự động

**Cách deploy:**

```bash
# Cài Google Cloud CLI
gcloud init

# Deploy
gcloud run deploy moc-vi-ai \
  --source . \
  --platform managed \
  --region asia-southeast1 \
  --allow-unauthenticated
```

---

### **Option 4: Vercel (Miễn phí, nhanh nhất)**

**Ưu điểm:**
- ✅ Deploy cực nhanh (~30s)
- ✅ CDN toàn cầu
- ✅ Auto-deploy từ GitHub
- ✅ Không giới hạn bandwidth

**Lưu ý:** Vercel giới hạn 10s/request, phù hợp với API nhanh

```bash
# Cài Vercel CLI
npm i -g vercel

# Deploy
vercel
```

---

### **🎯 Khuyến nghị:**

**Cho demo/test:** → **Render.com** (dễ nhất, miễn phí vĩnh viễn)

**Cho production:** → **Google Cloud Run** (mạnh nhất, 2M requests/tháng)

**Cho AI chatbot UI:** → **Hugging Face Spaces** (có giao diện đẹp sẵn)

## 📞 Hỗ trợ

Nếu gặp vấn đề, vui lòng:
1. Kiểm tra phần "Xử lý lỗi thường gặp"
2. Xem logs trong terminal hoặc Railway dashboard
3. Tạo issue trên GitHub

---

**Chúc bạn sử dụng vui vẻ! 🎉**

**Demo link:** *(Sẽ được cập nhật sau khi deploy)*
