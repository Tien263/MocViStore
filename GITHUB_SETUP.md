# 📤 Hướng Dẫn Push Lên GitHub

## Bước 1: Tạo Repository Trên GitHub

1. Truy cập [GitHub](https://github.com)
2. Đăng nhập vào tài khoản của bạn
3. Click nút **"New"** hoặc **"+"** → **"New repository"**
4. Điền thông tin:
   - **Repository name**: `MocViStore` hoặc `Exe_Demo`
   - **Description**: `Website bán hoa quả sấy Mộc Châu - ASP.NET Core MVC`
   - **Public** hoặc **Private** (tùy chọn)
   - **KHÔNG** check "Initialize this repository with a README"
5. Click **"Create repository"**

## Bước 2: Khởi Tạo Git Local

Mở **PowerShell** hoặc **Command Prompt** tại thư mục dự án:

```powershell
cd c:\Users\ADMIN\Desktop\Exe_Demo
```

### Khởi tạo Git repository
```bash
git init
```

### Thêm tất cả files vào staging
```bash
git add .
```

### Commit lần đầu
```bash
git commit -m "Initial commit: Mộc Vị Store - Hoa quả sấy Mộc Châu"
```

## Bước 3: Kết Nối Với GitHub

Thay `yourusername` và `repository-name` bằng thông tin của bạn:

```bash
git remote add origin https://github.com/yourusername/repository-name.git
```

Ví dụ:
```bash
git remote add origin https://github.com/johndoe/MocViStore.git
```

## Bước 4: Push Code Lên GitHub

### Đổi tên branch thành main (nếu cần)
```bash
git branch -M main
```

### Push code lên GitHub
```bash
git push -u origin main
```

**Lưu ý**: Nếu yêu cầu đăng nhập:
- Username: Tên đăng nhập GitHub của bạn
- Password: Sử dụng **Personal Access Token** (không phải mật khẩu)

## Bước 5: Tạo Personal Access Token (Nếu Cần)

1. Vào GitHub → **Settings** → **Developer settings**
2. Click **Personal access tokens** → **Tokens (classic)**
3. Click **Generate new token** → **Generate new token (classic)**
4. Điền thông tin:
   - **Note**: `MocViStore Token`
   - **Expiration**: Chọn thời gian hết hạn
   - **Select scopes**: Check ✅ **repo** (full control)
5. Click **Generate token**
6. **Copy token** và lưu lại (chỉ hiển thị 1 lần)
7. Sử dụng token này thay cho password khi push

## Bước 6: Kiểm Tra

Truy cập repository trên GitHub để xem code đã được push thành công.

## 📝 Các Lệnh Git Thường Dùng

### Kiểm tra trạng thái
```bash
git status
```

### Thêm file mới hoặc thay đổi
```bash
git add .
# hoặc
git add filename.cs
```

### Commit thay đổi
```bash
git commit -m "Mô tả thay đổi"
```

### Push lên GitHub
```bash
git push origin main
```

### Pull code mới nhất
```bash
git pull origin main
```

### Xem lịch sử commit
```bash
git log
```

### Tạo branch mới
```bash
git checkout -b feature/ten-tinh-nang
```

### Chuyển branch
```bash
git checkout main
```

### Merge branch
```bash
git merge feature/ten-tinh-nang
```

## 🔒 Bảo Mật

**QUAN TRỌNG**: Đảm bảo các file sau KHÔNG được push lên GitHub:

✅ Đã có trong `.gitignore`:
- `appsettings.Development.json` (chứa connection string, email password)
- `appsettings.Production.json`
- `bin/`, `obj/` folders
- `wwwroot/uploads/profiles/*` (ảnh người dùng)

### Nếu đã push nhầm file nhạy cảm:

1. Xóa file khỏi Git history:
```bash
git filter-branch --force --index-filter "git rm --cached --ignore-unmatch appsettings.Development.json" --prune-empty --tag-name-filter cat -- --all
```

2. Push force:
```bash
git push origin --force --all
```

3. Thay đổi tất cả passwords, tokens đã bị lộ

## 📋 Checklist Trước Khi Push

- [ ] Đã test ứng dụng chạy tốt
- [ ] Đã xóa các comment không cần thiết
- [ ] Đã kiểm tra `.gitignore`
- [ ] Đã xóa các file nhạy cảm
- [ ] Đã viết README.md rõ ràng
- [ ] Đã commit với message có ý nghĩa

## 🎯 Commit Message Convention

Sử dụng format:
```
<type>: <subject>

<body>
```

**Types**:
- `feat`: Tính năng mới
- `fix`: Sửa bug
- `docs`: Cập nhật documentation
- `style`: Format code, không ảnh hưởng logic
- `refactor`: Refactor code
- `test`: Thêm tests
- `chore`: Cập nhật build, dependencies

**Ví dụ**:
```bash
git commit -m "feat: Add product details page with cart functionality"
git commit -m "fix: Fix encoding issue for Vietnamese characters"
git commit -m "docs: Update README with installation guide"
```

## 🚀 Sau Khi Push

1. Thêm **Topics/Tags** cho repository:
   - `aspnet-core`
   - `mvc`
   - `ecommerce`
   - `csharp`
   - `entity-framework`

2. Thêm **Description** ngắn gọn

3. Thêm **Website URL** (nếu đã deploy)

4. Tạo **Releases** cho các version quan trọng

5. Viết **Wiki** nếu cần hướng dẫn chi tiết

## 💡 Tips

- Commit thường xuyên với message rõ ràng
- Tạo branch riêng cho mỗi tính năng mới
- Không commit code chưa test
- Review code trước khi merge vào main
- Sử dụng Pull Request cho team work

---

**Happy Coding! 🎉**
