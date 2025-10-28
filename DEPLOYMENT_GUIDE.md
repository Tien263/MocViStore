# 🚀 Hướng Dẫn Deploy Mộc Vị Store Lên Internet

## Mục Lục
1. [Deploy Web App lên Azure](#deploy-web-app-lên-azure)
2. [Deploy Database lên Azure SQL](#deploy-database-lên-azure-sql)
3. [Deploy AI Service lên Railway/Render](#deploy-ai-service)
4. [Cấu hình Domain](#cấu-hình-domain)
5. [Troubleshooting](#troubleshooting)

---

## 1️⃣ Deploy Web App Lên Azure (MIỄN PHÍ 12 tháng)

### Bước 1: Đăng ký Azure
1. Truy cập: https://azure.microsoft.com/free/
2. Click "Start free"
3. Đăng nhập bằng Microsoft Account (hoặc tạo mới)
4. Nhập thông tin thẻ tín dụng (không bị trừ tiền, chỉ để xác minh)
5. Nhận $200 credit + 12 tháng dịch vụ miễn phí

### Bước 2: Cài đặt Azure CLI
```bash
# Download và cài đặt từ:
https://aka.ms/installazurecliwindows

# Sau khi cài xong, mở PowerShell và login:
az login
```

### Bước 3: Tạo Resource Group
```bash
# Tạo resource group
az group create --name MocViStore-RG --location southeastasia

# Verify
az group list --output table
```

### Bước 4: Tạo App Service Plan (Free Tier)
```bash
# Tạo App Service Plan miễn phí
az appservice plan create --name MocViStore-Plan --resource-group MocViStore-RG --sku F1 --is-linux

# F1 = Free tier (1GB RAM, 1GB storage)
```

### Bước 5: Tạo Web App
```bash
# Tạo Web App với .NET 8
az webapp create --resource-group MocViStore-RG --plan MocViStore-Plan --name mocvistore --runtime "DOTNET|8.0"

# Tên "mocvistore" phải unique toàn Azure
# URL sẽ là: https://mocvistore.azurewebsites.net
```

### Bước 6: Cấu hình Connection String
```bash
# Set connection string cho Azure SQL
az webapp config connection-string set --resource-group MocViStore-RG --name mocvistore --settings DefaultConnection="Server=tcp:mocvistore-server.database.windows.net,1433;Initial Catalog=MocViStoreDB;Persist Security Info=False;User ID=sqladmin;Password=YourPassword123!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;" --connection-string-type SQLAzure
```

### Bước 7: Cấu hình App Settings
```bash
# Set các biến môi trường
az webapp config appsettings set --resource-group MocViStore-RG --name mocvistore --settings \
  ASPNETCORE_ENVIRONMENT=Production \
  EmailSettings__SmtpServer=smtp.gmail.com \
  EmailSettings__SmtpPort=587 \
  EmailSettings__SenderEmail=your-email@gmail.com \
  EmailSettings__SenderPassword=your-app-password
```

### Bước 8: Deploy Code
```bash
# Từ thư mục project
cd C:\Users\ADMIN\Desktop\Exe_Demo

# Build project
dotnet publish -c Release -o ./publish

# Deploy lên Azure
az webapp deployment source config-zip --resource-group MocViStore-RG --name mocvistore --src ./publish.zip
```

**Hoặc deploy từ GitHub:**
```bash
# Kết nối với GitHub repo
az webapp deployment source config --name mocvistore --resource-group MocViStore-RG --repo-url https://github.com/Tien263/MocViStore --branch main --manual-integration
```

---

## 2️⃣ Deploy Database Lên Azure SQL

### Bước 1: Tạo Azure SQL Server
```bash
# Tạo SQL Server
az sql server create --name mocvistore-server --resource-group MocViStore-RG --location southeastasia --admin-user sqladmin --admin-password YourPassword123!

# Cho phép Azure services truy cập
az sql server firewall-rule create --resource-group MocViStore-RG --server mocvistore-server --name AllowAzureServices --start-ip-address 0.0.0.0 --end-ip-address 0.0.0.0

# Cho phép IP của bạn truy cập (để quản lý)
az sql server firewall-rule create --resource-group MocViStore-RG --server mocvistore-server --name AllowMyIP --start-ip-address YOUR_IP --end-ip-address YOUR_IP
```

### Bước 2: Tạo Database (Free Tier)
```bash
# Tạo database với Basic tier (5 DTU, 2GB)
az sql db create --resource-group MocViStore-RG --server mocvistore-server --name MocViStoreDB --service-objective Basic --max-size 2GB
```

### Bước 3: Import Database
```bash
# Export database local ra file .bacpac
sqlpackage /Action:Export /SourceServerName:localhost /SourceDatabaseName:MocViStoreDB /TargetFile:MocViStoreDB.bacpac

# Upload lên Azure Storage (tạo storage account trước)
az storage account create --name mocvistorestorage --resource-group MocViStore-RG --location southeastasia --sku Standard_LRS

# Upload file
az storage blob upload --account-name mocvistorestorage --container-name backups --name MocViStoreDB.bacpac --file MocViStoreDB.bacpac

# Import vào Azure SQL
az sql db import --resource-group MocViStore-RG --server mocvistore-server --name MocViStoreDB --storage-key-type StorageAccessKey --storage-key YOUR_STORAGE_KEY --storage-uri https://mocvistorestorage.blob.core.windows.net/backups/MocViStoreDB.bacpac --admin-user sqladmin --admin-password YourPassword123!
```

**Hoặc dùng SQL Server Management Studio:**
1. Connect tới Azure SQL Server
2. Right-click database → Tasks → Import Data-tier Application
3. Chọn file .bacpac
4. Follow wizard

---

## 3️⃣ Deploy AI Service Lên Railway (MIỄN PHÍ)

### Bước 1: Đăng ký Railway
1. Truy cập: https://railway.app
2. Sign up với GitHub
3. Verify email

### Bước 2: Tạo Project
1. Click "New Project"
2. Chọn "Deploy from GitHub repo"
3. Chọn repository: Tien263/MocViStore
4. Railway sẽ tự động detect Python app

### Bước 3: Cấu hình Environment Variables
```bash
# Trong Railway dashboard, thêm variables:
GEMINI_API_KEY=your-gemini-api-key
PORT=8000
PYTHON_VERSION=3.11
```

### Bước 4: Tạo railway.json
```json
{
  "build": {
    "builder": "NIXPACKS",
    "buildCommand": "cd Trainning_AI && pip install -r requirements.txt"
  },
  "deploy": {
    "startCommand": "cd Trainning_AI && python -m app.main",
    "restartPolicyType": "ON_FAILURE",
    "restartPolicyMaxRetries": 10
  }
}
```

### Bước 5: Deploy
1. Push code lên GitHub
2. Railway tự động build và deploy
3. Lấy URL: https://your-app.railway.app

### Bước 6: Cập nhật Web App
```bash
# Update AI API URL trong Azure Web App
az webapp config appsettings set --resource-group MocViStore-RG --name mocvistore --settings \
  AI_API_URL=https://your-app.railway.app
```

---

## 4️⃣ Cấu Hình Domain Tùy Chỉnh (Optional)

### Option 1: Dùng Domain Miễn Phí từ Freenom
1. Truy cập: https://www.freenom.com
2. Tìm domain miễn phí (.tk, .ml, .ga, .cf, .gq)
3. Đăng ký domain (VD: mocvistore.tk)

### Option 2: Mua Domain từ Namecheap/GoDaddy
1. Mua domain .com/.vn (khoảng $10-15/năm)

### Cấu Hình DNS
1. Vào DNS Management của domain
2. Thêm CNAME record:
   - Name: www
   - Value: mocvistore.azurewebsites.net
   - TTL: 3600

3. Thêm A record (lấy IP từ Azure):
```bash
# Lấy IP của Web App
az webapp show --resource-group MocViStore-RG --name mocvistore --query outboundIpAddresses --output tsv
```

4. Trong Azure Portal:
   - Vào Web App → Custom domains
   - Click "Add custom domain"
   - Nhập domain của bạn
   - Verify ownership

### Cấu Hình SSL (HTTPS)
```bash
# Azure tự động cung cấp SSL certificate miễn phí
az webapp config ssl bind --resource-group MocViStore-RG --name mocvistore --certificate-thumbprint auto --ssl-type SNI
```

---

## 5️⃣ Kiểm Tra & Monitoring

### Kiểm tra Web App
```bash
# Check status
az webapp show --resource-group MocViStore-RG --name mocvistore --query state

# View logs
az webapp log tail --resource-group MocViStore-RG --name mocvistore

# Restart app
az webapp restart --resource-group MocViStore-RG --name mocvistore
```

### Kiểm tra Database
```bash
# Test connection
sqlcmd -S mocvistore-server.database.windows.net -U sqladmin -P YourPassword123! -d MocViStoreDB -Q "SELECT COUNT(*) FROM Products"
```

### Setup Application Insights (Monitoring)
```bash
# Tạo Application Insights
az monitor app-insights component create --app mocvistore-insights --location southeastasia --resource-group MocViStore-RG --application-type web

# Link với Web App
az webapp config appsettings set --resource-group MocViStore-RG --name mocvistore --settings \
  APPLICATIONINSIGHTS_CONNECTION_STRING=$(az monitor app-insights component show --app mocvistore-insights --resource-group MocViStore-RG --query connectionString -o tsv)
```

---

## 6️⃣ Troubleshooting

### Lỗi: Web App không start
```bash
# Check logs
az webapp log tail --resource-group MocViStore-RG --name mocvistore

# Common issues:
# 1. Connection string sai
# 2. Thiếu environment variables
# 3. Port binding sai (phải dùng port 8080 trên Azure)
```

### Lỗi: Database connection timeout
```bash
# Check firewall rules
az sql server firewall-rule list --resource-group MocViStore-RG --server mocvistore-server

# Add your IP
az sql server firewall-rule create --resource-group MocViStore-RG --server mocvistore-server --name AllowMyNewIP --start-ip-address YOUR_IP --end-ip-address YOUR_IP
```

### Lỗi: AI Service không hoạt động
```bash
# Check Railway logs
# Vào Railway dashboard → Deployments → View logs

# Common issues:
# 1. Thiếu GEMINI_API_KEY
# 2. Port không đúng
# 3. Dependencies không cài đủ
```

---

## 7️⃣ Chi Phí Ước Tính

### Miễn Phí (12 tháng đầu)
- Azure Web App: F1 tier (Free)
- Azure SQL Database: Basic tier ($5/tháng, có trong $200 credit)
- Railway: 500 hours/month free
- **Tổng: $0** (trong 12 tháng)

### Sau 12 tháng
- Azure Web App: B1 tier (~$13/tháng)
- Azure SQL Database: Basic tier (~$5/tháng)
- Railway: $5/tháng (nếu vượt free tier)
- **Tổng: ~$23/tháng**

---

## 8️⃣ Backup & Restore

### Backup Database
```bash
# Auto backup (Azure SQL tự động backup)
az sql db show --resource-group MocViStore-RG --server mocvistore-server --name MocViStoreDB --query earliestRestoreDate

# Manual backup
az sql db export --resource-group MocViStore-RG --server mocvistore-server --name MocViStoreDB --admin-user sqladmin --admin-password YourPassword123! --storage-key YOUR_KEY --storage-key-type StorageAccessKey --storage-uri https://mocvistorestorage.blob.core.windows.net/backups/backup.bacpac
```

### Restore Database
```bash
az sql db restore --resource-group MocViStore-RG --server mocvistore-server --name MocViStoreDB --dest-name MocViStoreDB-Restored --time "2025-01-01T00:00:00Z"
```

---

## 9️⃣ Security Best Practices

1. **Không commit secrets vào Git**
   - Dùng Azure Key Vault
   - Dùng Environment Variables

2. **Enable HTTPS Only**
```bash
az webapp update --resource-group MocViStore-RG --name mocvistore --https-only true
```

3. **Enable Authentication**
```bash
az webapp auth update --resource-group MocViStore-RG --name mocvistore --enabled true --action LoginWithAzureActiveDirectory
```

4. **Regular Updates**
   - Update dependencies
   - Apply security patches
   - Monitor logs

---

## 🎉 Hoàn Thành!

Website của bạn giờ đã online tại:
- **Web App**: https://mocvistore.azurewebsites.net
- **Custom Domain**: https://www.mocvistore.tk (nếu có)

Mọi người có thể truy cập và sử dụng như các website bình thường!

---

## 📞 Support

Nếu gặp vấn đề, liên hệ:
- Email: support@mocvi.vn
- GitHub Issues: https://github.com/Tien263/MocViStore/issues
