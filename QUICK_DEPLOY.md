# 🚀 Hướng Dẫn Deploy Nhanh - 15 Phút

## ✅ Chuẩn Bị

### 1. Tạo tài khoản (MIỄN PHÍ)
- [ ] Azure: https://azure.microsoft.com/free/ (Miễn phí 12 tháng + $200 credit)
- [ ] Railway: https://railway.app (Miễn phí 500 hours/tháng)

### 2. Cài đặt công cụ
- [ ] Azure CLI: https://aka.ms/installazurecliwindows
- [ ] Git: https://git-scm.com/download/win

---

## 🎯 Phương Án 1: Deploy Tự Động (Khuyên Dùng)

### Bước 1: Chạy script tự động
```bash
# Mở PowerShell trong thư mục project
cd C:\Users\ADMIN\Desktop\Exe_Demo

# Chạy script deploy
.\deploy-to-azure.bat
```

### Bước 2: Nhập thông tin khi được hỏi
- Resource Group: `MocViStore-RG` (hoặc tên bạn muốn)
- App Name: `mocvistore-yourname` (phải unique)
- Location: `southeastasia` (gần Việt Nam nhất)

### Bước 3: Đợi deploy xong (5-10 phút)
Script sẽ tự động:
- ✅ Tạo Resource Group
- ✅ Tạo App Service Plan
- ✅ Tạo Web App
- ✅ Build project
- ✅ Deploy code

### Bước 4: Truy cập website
```
https://mocvistore-yourname.azurewebsites.net
```

---

## 🎯 Phương Án 2: Deploy Từ GitHub (Dễ Nhất)

### Bước 1: Push code lên GitHub (đã làm rồi ✅)
```bash
git push origin main
```

### Bước 2: Vào Azure Portal
1. Truy cập: https://portal.azure.com
2. Đăng nhập với tài khoản Azure

### Bước 3: Tạo Web App từ Portal
1. Click "Create a resource"
2. Tìm "Web App" → Click "Create"
3. Điền thông tin:
   - **Resource Group**: Tạo mới "MocViStore-RG"
   - **Name**: mocvistore-yourname (phải unique)
   - **Publish**: Code
   - **Runtime stack**: .NET 8 (LTS)
   - **Operating System**: Linux
   - **Region**: Southeast Asia
   - **Pricing plan**: Free F1 (1 GB RAM, 1 GB storage)
4. Click "Review + create" → "Create"

### Bước 4: Kết nối với GitHub
1. Vào Web App vừa tạo
2. Sidebar → "Deployment Center"
3. Source: GitHub
4. Authorize Azure to access GitHub
5. Chọn:
   - Organization: Tien263
   - Repository: MocViStore
   - Branch: main
6. Click "Save"

### Bước 5: Đợi deploy tự động (5-10 phút)
Azure sẽ tự động:
- ✅ Clone code từ GitHub
- ✅ Build project
- ✅ Deploy lên server

### Bước 6: Kiểm tra
```
https://mocvistore-yourname.azurewebsites.net
```

---

## 🗄️ Deploy Database (Bắt Buộc)

### Option 1: Azure SQL Database (Khuyên dùng)

#### Bước 1: Tạo SQL Server
1. Azure Portal → "Create a resource"
2. Tìm "SQL Database" → "Create"
3. Điền thông tin:
   - **Resource Group**: MocViStore-RG
   - **Database name**: MocViStoreDB
   - **Server**: Create new
     - Server name: mocvistore-server
     - Admin login: sqladmin
     - Password: YourPassword123!
     - Location: Southeast Asia
   - **Compute + storage**: Basic (5 DTU, 2GB) - $5/tháng
4. Click "Review + create" → "Create"

#### Bước 2: Cấu hình Firewall
1. Vào SQL Server → "Networking"
2. Add firewall rule:
   - Name: AllowAzureServices
   - Start IP: 0.0.0.0
   - End IP: 0.0.0.0
3. Add your IP: Click "Add client IP"
4. Save

#### Bước 3: Import Database
**Cách 1: Dùng SQL Server Management Studio**
1. Connect tới Azure SQL Server:
   - Server: mocvistore-server.database.windows.net
   - Login: sqladmin
   - Password: YourPassword123!
2. Right-click "Databases" → "Import Data-tier Application"
3. Chọn file .bacpac (export từ local trước)
4. Follow wizard

**Cách 2: Dùng Azure Data Studio**
1. Download: https://aka.ms/azuredatastudio
2. Connect tới Azure SQL
3. Import database

#### Bước 4: Update Connection String
1. Vào Web App → "Configuration"
2. Connection strings → "New connection string"
3. Name: `DefaultConnection`
4. Value:
```
Server=tcp:mocvistore-server.database.windows.net,1433;Initial Catalog=MocViStoreDB;Persist Security Info=False;User ID=sqladmin;Password=YourPassword123!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;
```
5. Type: SQLAzure
6. Save → Restart Web App

### Option 2: Dùng Database Miễn Phí (Giới hạn)

**Supabase (PostgreSQL - Miễn phí)**
1. Truy cập: https://supabase.com
2. Create project
3. Lấy connection string
4. Cần chuyển đổi code từ SQL Server sang PostgreSQL

---

## 🤖 Deploy AI Service

### Bước 1: Deploy lên Railway
1. Truy cập: https://railway.app
2. Sign up với GitHub
3. "New Project" → "Deploy from GitHub repo"
4. Chọn: Tien263/MocViStore
5. Railway tự động detect Python

### Bước 2: Cấu hình Environment
1. Vào project → "Variables"
2. Thêm:
   - `GEMINI_API_KEY`: your-api-key
   - `PORT`: 8000
3. Save

### Bước 3: Tạo Procfile
Tạo file `Procfile` trong thư mục `Trainning_AI`:
```
web: cd Trainning_AI && python -m app.main
```

### Bước 4: Deploy
1. Push code lên GitHub
2. Railway tự động deploy
3. Lấy URL: https://your-app.railway.app

### Bước 5: Update Web App
1. Azure Portal → Web App → Configuration
2. Application settings → New
3. Name: `AI__ApiUrl`
4. Value: `https://your-app.railway.app`
5. Save → Restart

---

## 🌐 Cấu Hình Domain (Optional)

### Option 1: Dùng Domain Miễn Phí
1. Freenom: https://www.freenom.com
2. Đăng ký domain .tk, .ml, .ga (miễn phí)
3. Cấu hình DNS:
   - CNAME: www → mocvistore-yourname.azurewebsites.net

### Option 2: Mua Domain
1. Namecheap/GoDaddy: Mua .com/.vn (~$10-15/năm)
2. Cấu hình DNS tương tự

### Thêm Domain vào Azure
1. Web App → "Custom domains"
2. "Add custom domain"
3. Nhập domain
4. Verify ownership
5. Azure tự động cấp SSL certificate (HTTPS)

---

## ✅ Checklist Sau Khi Deploy

- [ ] Website truy cập được
- [ ] Database kết nối thành công
- [ ] Đăng ký tài khoản mới được
- [ ] Đăng nhập được
- [ ] Thêm sản phẩm vào giỏ hàng
- [ ] Checkout được
- [ ] Email gửi được
- [ ] AI Chat hoạt động
- [ ] Voucher áp dụng được
- [ ] Staff panel truy cập được

---

## 🐛 Troubleshooting

### Lỗi: Website hiển thị "Service Unavailable"
**Giải pháp:**
1. Vào Web App → "Diagnose and solve problems"
2. Check logs: "Application Logs"
3. Common issues:
   - Connection string sai
   - Thiếu environment variables
   - Database không kết nối được

### Lỗi: Database connection timeout
**Giải pháp:**
1. Check firewall rules
2. Thêm IP của Azure Web App vào whitelist
3. Verify connection string

### Lỗi: AI Service không hoạt động
**Giải pháp:**
1. Check Railway logs
2. Verify GEMINI_API_KEY
3. Check AI_API_URL trong Web App settings

---

## 💰 Chi Phí

### Miễn Phí (12 tháng đầu với Azure)
- Web App: F1 tier (Free)
- SQL Database: Basic tier ($5/tháng, có trong $200 credit)
- Railway: 500 hours/month free
- **Tổng: $0**

### Sau 12 tháng
- Web App: B1 tier (~$13/tháng)
- SQL Database: Basic tier (~$5/tháng)
- Railway: $5/tháng
- **Tổng: ~$23/tháng**

---

## 📞 Hỗ Trợ

Nếu gặp vấn đề:
1. Xem file `DEPLOYMENT_GUIDE.md` (chi tiết hơn)
2. Check Azure logs
3. GitHub Issues: https://github.com/Tien263/MocViStore/issues

---

## 🎉 Hoàn Thành!

Website của bạn đã online! Chia sẻ link cho bạn bè:
```
https://mocvistore-yourname.azurewebsites.net
```

Mọi người có thể:
- ✅ Xem sản phẩm
- ✅ Đăng ký tài khoản
- ✅ Mua hàng
- ✅ Chat với AI
- ✅ Sử dụng voucher

**Chúc mừng! 🎊**
