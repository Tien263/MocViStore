import os
from dotenv import load_dotenv

load_dotenv()

class Settings:
    # API Keys
    OPENAI_API_KEY = os.getenv("OPENAI_API_KEY", "")
    
    # Server Config
    HOST = os.getenv("HOST", "0.0.0.0")
    PORT = int(os.getenv("PORT", 8000))
    
    # Database Config
    CHROMA_DB_PATH = "./chroma_db"
    COLLECTION_NAME = "moc_chau_fruits"
    
    # Model Config - Use smaller model for low memory environments
    EMBEDDING_MODEL = os.getenv("EMBEDDING_MODEL", "sentence-transformers/all-MiniLM-L6-v2")  # Lighter model: ~80MB
    LLM_MODEL = "gpt-3.5-turbo"  # Hoặc sử dụng model khác
    
    # RAG Config
    CHUNK_SIZE = 500
    CHUNK_OVERLAP = 50
    TOP_K_RESULTS = 3
    
    # Data Paths - Multiple JSON files
    DATA_PATH = "./data/moc_chau_fruits.json"  # Main products
    BRAND_INFO_PATH = "./data/brand_info.json"
    SEASONAL_CALENDAR_PATH = "./data/seasonal_calendar.json"
    STORAGE_GUIDE_PATH = "./data/storage_guide.json"

settings = Settings()
