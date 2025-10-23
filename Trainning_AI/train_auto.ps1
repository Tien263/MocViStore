# Set UTF-8 encoding
[Console]::OutputEncoding = [System.Text.Encoding]::UTF8
$env:PYTHONIOENCODING = "utf-8"

# Run training script with auto-confirmation
Write-Host "y" | & "venv\Scripts\python.exe" "train.py"
