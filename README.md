# 🍓 Mộc Vị Store - Hoa Quả Sấy Mộc Châu

## 📖 Giới Thiệu

**Mộc Vị Store** là website thương mại điện tử chuyên bán hoa quả sấy cao cấp từ Mộc Châu. Website được xây dựng bằng **ASP.NET Core MVC** với giao diện hiện đại, thân thiện và đầy đủ tính năng quản lý bán hàng.

## ✨ Tính Năng Chính

### 🔐 Hệ Thống Xác Thực & Bảo Mật
- ✅ Đăng ký tài khoản với xác thực OTP qua email
- ✅ Đăng nhập bằng tài khoản hoặc Google OAuth
- ✅ Quên mật khẩu với OTP verification
- ✅ Bảo mật session và cookie
- ✅ Authorization cho các trang yêu cầu đăng nhập

### 👤 Quản Lý Profile
- ✅ Xem và chỉnh sửa thông tin cá nhân
- ✅ Upload và thay đổi ảnh đại diện
- ✅ Hiển thị thông tin khách hàng (mã KH, điểm thưởng)
- ✅ Lịch sử đăng nhập

### 🛍️ Hệ Thống Sản Phẩm
- ✅ Hiển thị danh sách sản phẩm với 4 danh mục:
  - Sản phẩm sấy dẻo (200g)
  - Sản phẩm sấy giòn (200g)
  - Sản phẩm sấy thăng hoa (100g)
  - Mini size mix (50g)
- ✅ Lọc sản phẩm theo danh mục
- ✅ Tìm kiếm sản phẩm
- ✅ Sắp xếp theo giá, tên, mới nhất
- ✅ Chi tiết sản phẩm với đầy đủ thông tin
- ✅ Sản phẩm liên quan
- ✅ Rating và đánh giá

### 🛒 Giỏ Hàng
- ✅ Thêm sản phẩm vào giỏ hàng
- ✅ Cập nhật số lượng
- ✅ Xóa sản phẩm khỏi giỏ
- ✅ Tính tổng tiền tự động
- ✅ Lưu giỏ hàng vào database

### 🎨 Giao Diện
- ✅ Responsive design (Desktop, Tablet, Mobile)
- ✅ Background ảnh đẹp với parallax effect
- ✅ Overlay màu xanh đặc trưng
- ✅ Animation mượt mà
- ✅ Font tiếng Việt chuẩn

## 🛠️ Công Nghệ Sử Dụng

### Backend
- **Framework**: ASP.NET Core 8.0 MVC
- **Database**: SQL Server
- **ORM**: Entity Framework Core
- **Authentication**: ASP.NET Core Identity, Google OAuth
- **Email Service**: SMTP (Gmail)

### Frontend
- **HTML5, CSS3, JavaScript**
- **Bootstrap 4**
- **jQuery**
- **Font Awesome**
- **Owl Carousel**
- **Magnific Popup**

## 📁 Cấu Trúc Dự Án

```
Exe_Demo/
├── Controllers/
│   ├── AuthController.cs          # Xác thực, đăng ký, đăng nhập
│   ├── ProfileController.cs       # Quản lý profile
│   ├── ProductController.cs       # Quản lý sản phẩm
│   ├── CartController.cs          # Giỏ hàng
│   ├── HomeController.cs          # Trang chủ
│   ├── AboutController.cs         # Giới thiệu
│   ├── BlogController.cs          # Blog
│   └── ContactController.cs       # Liên hệ
├── Models/
│   ├── User.cs                    # Model người dùng
│   ├── Customer.cs                # Model khách hàng
│   ├── Product.cs                 # Model sản phẩm
│   ├── Category.cs                # Model danh mục
│   ├── Cart.cs                    # Model giỏ hàng
│   ├── Order.cs                   # Model đơn hàng
│   └── ViewModels/                # ViewModels
├── Views/
│   ├── Shared/
│   │   └── _Layout.cshtml         # Layout chung
│   ├── Auth/                      # Views xác thực
│   ├── Profile/                   # Views profile
│   ├── Product/                   # Views sản phẩm
│   ├── Cart/                      # Views giỏ hàng
│   └── Home/                      # Views trang chủ
├── Data/
│   └── ApplicationDbContext.cs    # Database context
├── Services/
│   └── EmailService.cs            # Service gửi email
├── wwwroot/
│   ├── css/                       # CSS files
│   ├── js/                        # JavaScript files
│   ├── images/                    # Hình ảnh
│   └── uploads/                   # Upload files
└── SQL_Scripts/
    └── InsertProductsData.sql     # Script insert dữ liệu
```

## 📊 Database Schema

### Bảng Chính
- **Users**: Thông tin người dùng
- **Customers**: Thông tin khách hàng
- **Categories**: Danh mục sản phẩm (4 loại)
- **Products**: Sản phẩm (18 sản phẩm)
- **Carts**: Giỏ hàng
- **Orders**: Đơn hàng
- **OrderDetails**: Chi tiết đơn hàng
- **OtpVerifications**: Xác thực OTP

## 🚀 Cài Đặt & Chạy Dự Án

### Yêu Cầu
- .NET 8.0 SDK
- SQL Server
- Visual Studio 2022 hoặc VS Code

### Các Bước Cài Đặt

1. **Clone repository**
```bash
git clone https://github.com/yourusername/Exe_Demo.git
cd Exe_Demo
```

2. **Cấu hình Database**
- Mở `appsettings.json`
- Cập nhật connection string:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=MocViStoreDB;Trusted_Connection=True;TrustServerCertificate=True"
}
```

3. **Chạy Migration**
```bash
dotnet ef database update
```

4. **Insert dữ liệu mẫu**
```bash
sqlcmd -S localhost -d MocViStoreDB -i SQL_Scripts/InsertProductsData.sql -f 65001
```

5. **Cấu hình Email Service**
- Cập nhật thông tin email trong `appsettings.json`:
```json
"EmailSettings": {
  "SmtpServer": "smtp.gmail.com",
  "SmtpPort": 587,
  "SenderEmail": "your-email@gmail.com",
  "SenderPassword": "your-app-password"
}
```

6. **Cấu hình Google OAuth (Optional)**
- Tạo OAuth credentials tại [Google Cloud Console](https://console.cloud.google.com)
- Cập nhật trong `appsettings.json`:
```json
"Authentication": {
  "Google": {
    "ClientId": "your-client-id",
    "ClientSecret": "your-client-secret"
  }
}
```

7. **Chạy ứng dụng**
```bash
dotnet run
```

8. **Truy cập**
```
http://localhost:5241
```

## 📦 Danh Sách Sản Phẩm

### Sản Phẩm Sấy Dẻo (200g)
- Mận Sấy Dẻo - 65,000đ
- Xoài Sấy Dẻo - 70,000đ
- Đào Sấy Dẻo - 65,000đ
- Dâu Sấy Dẻo - 90,000đ
- Hồng Sấy Dẻo - 95,000đ

### Sản Phẩm Sấy Giòn (200g)
- Mít Sấy Giòn - 80,000đ
- Chuối Sấy Giòn - 80,000đ

### Sản Phẩm Sấy Thăng Hoa (100g)
- Dâu Sấy Thăng Hoa - 140,000đ
- Sữa Chua Sấy Thăng Hoa - 95,000đ

### Mini Size Mix (50g) - Tối thiểu 4 pack
- Mận Mini - 18,000đ
- Xoài Mini - 20,000đ
- Đào Mini - 18,000đ
- Dâu Mini - 25,000đ
- Hồng Mini - 28,000đ
- Mít Mini - 22,000đ
- Chuối Mini - 22,000đ
- Dâu Thăng Hoa Mini - 75,000đ
- Sữa Chua Thăng Hoa Mini - 50,000đ

## 🎯 Tính Năng Sắp Tới

- [ ] Thanh toán online (VNPay, Momo)
- [ ] Quản lý đơn hàng
- [ ] Theo dõi vận chuyển
- [ ] Đánh giá và review sản phẩm
- [ ] Wishlist
- [ ] Voucher và khuyến mãi
- [ ] Admin dashboard
- [ ] Báo cáo thống kê

## 📸 Screenshots

### Trang Chủ
![Home Page](screenshots/home.png)

### Sản Phẩm
![Products Page](screenshots/products.png)

### Chi Tiết Sản Phẩm
![Product Details](screenshots/product-details.png)

### Profile
![Profile Page](screenshots/profile.png)

## 👥 Đóng Góp

Mọi đóng góp đều được chào đón! Vui lòng:
1. Fork repository
2. Tạo branch mới (`git checkout -b feature/AmazingFeature`)
3. Commit changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to branch (`git push origin feature/AmazingFeature`)
5. Tạo Pull Request

## 📝 License

Dự án này được phát hành dưới giấy phép MIT. Xem file `LICENSE` để biết thêm chi tiết.

## 📞 Liên Hệ

- **Website**: [mocvi.vn](https://mocvi.vn)
- **Email**: support@mocvi.vn
- **Phone**: +84 912 345 678
- **Address**: Số 123, Mộc Châu, Sơn La, Việt Nam

## 🙏 Cảm Ơn

- Template gốc: [Liquor Store Template by Colorlib](https://colorlib.com)
- Icons: [Font Awesome](https://fontawesome.com)
- Fonts: [Google Fonts](https://fonts.google.com)

---

**Made with ❤️ by Mộc Vị Team**
