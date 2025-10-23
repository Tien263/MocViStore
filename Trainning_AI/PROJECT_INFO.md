# 📋 Thông tin dự án

## Tên dự án
**Mộc Châu Fruits AI API**

## Mô tả
RESTful API cho AI chatbot được training với dữ liệu tùy chỉnh về hoa quả Mộc Châu. Sử dụng công nghệ RAG (Retrieval-Augmented Generation) để trả lời câu hỏi dựa trên kiến thức được cung cấp.

## Công nghệ sử dụng

### Backend Framework
- **FastAPI** - Modern Python web framework
- **Uvicorn** - ASGI server

### AI/ML
- **ChromaDB** - Vector database
- **Sentence Transformers** - Embedding model (multilingual)
- **OpenAI GPT** - LLM (tùy chọn)
- **LangChain** - RAG framework

### Python Version
- Python 3.8+

## Cấu trúc dự án

```
Trainning_AI/
├── app/                     # Source code chính
│   ├── main.py             # FastAPI app & endpoints
│   ├── config.py           # Configuration
│   ├── vector_store.py     # Vector DB logic
│   └── llm_service.py      # LLM integration
├── data/                    # Dữ liệu training
│   └── moc_chau_fruits.json
├── chroma_db/              # Vector database (auto-generated)
├── frontend/               # (Optional - có thể xóa nếu chỉ dùng API)
├── requirements.txt        # Dependencies
├── requirements-dev.txt    # Dev dependencies
├── train.py               # Training script
├── test_api.py            # API testing script
├── api_examples.http      # REST Client examples
└── setup.bat              # Setup script
```

## API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/` | API information |
| GET | `/docs` | Swagger UI documentation |
| GET | `/redoc` | ReDoc documentation |
| GET | `/api/health` | Health check |
| POST | `/api/chat` | Chat with AI |
| GET | `/api/fruits` | Get all fruits data |
| POST | `/api/train/add` | Add new fruit data |
| POST | `/api/train/reload` | Reload data from JSON |

## Cách chạy

### Development
```bash
# Setup
setup.bat

# Train
python train.py

# Run
run.bat
```

### Production
```bash
# Install dependencies
pip install -r requirements.txt

# Train data
python train.py

# Run with production settings
uvicorn app.main:app --host 0.0.0.0 --port 8000 --workers 4
```

## Environment Variables

```env
# Optional - OpenAI API Key
OPENAI_API_KEY=sk-xxx

# Server Config
HOST=0.0.0.0
PORT=8000
```

## Tính năng chính

1. **RAG System** - Retrieval-Augmented Generation
2. **Vector Search** - Semantic search với embeddings
3. **Multilingual** - Hỗ trợ tiếng Việt
4. **Auto Documentation** - Swagger UI & ReDoc
5. **Easy Training** - Chỉ cần JSON file
6. **RESTful API** - Dễ tích hợp

## Use Cases

- Chatbot cho website thương mại điện tử
- API cho mobile app
- Knowledge base search
- Q&A system
- Customer support automation

## Mở rộng

### Thêm dữ liệu mới
1. Chỉnh sửa `data/moc_chau_fruits.json`
2. Chạy `python train.py`

### Thay đổi domain
1. Chuẩn bị dữ liệu JSON theo format
2. Update `DATA_PATH` trong `config.py`
3. Train lại

### Deploy
- Docker
- Heroku
- AWS Lambda
- Google Cloud Run
- Azure App Service

## Performance

- **Embedding**: ~100ms
- **Search**: <50ms
- **LLM Response**: 1-3s (với OpenAI)
- **Total**: ~1-3s per request

## Bảo mật

- CORS configuration
- API key management
- Rate limiting (cần thêm)
- Authentication (cần thêm nếu cần)

## License
MIT License

## Tác giả
AI Assistant

## Ngày tạo
2025-10-10
