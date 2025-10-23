# Set UTF-8 encoding
[Console]::OutputEncoding = [System.Text.Encoding]::UTF8
$env:PYTHONIOENCODING = "utf-8"

# Run FastAPI server
& "venv\Scripts\python.exe" "-m" "uvicorn" "app.main:app" "--reload" "--host" "0.0.0.0" "--port" "8000"
