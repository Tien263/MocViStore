# Hướng Dẫn Sử Dụng Hệ Thống Quản Lý Staff

## Tổng Quan

Hệ thống quản lý Staff được xây dựng để hỗ trợ nhân viên trong việc quản lý cửa hàng, bao gồm:
- **Dashboard**: Tổng quan về doanh thu, đơn hàng
- **Quản lý sản phẩm**: CRUD sản phẩm, quản lý tồn kho
- **Quản lý đơn hàng**: Xem và cập nhật trạng thái đơn hàng
- **Bán hàng trực tiếp**: Tạo đơn hàng tại quầy
- **Thống kê doanh số**: Báo cáo chi tiết về doanh thu, lợi nhuận

## Cấu Trúc Hệ Thống

### 1. Models & ViewModels

Đã tạo các ViewModels trong `Models/ViewModels/`:
- `StaffDashboardViewModel.cs`: Dữ liệu cho dashboard
- `ProductManagementViewModel.cs`: Quản lý sản phẩm
- `OrderManagementViewModel.cs`: Quản lý đơn hàng
- `DirectSaleViewModel.cs`: Bán hàng trực tiếp
- `SalesReportViewModel.cs`: Thống kê doanh số

### 2. Controller

**StaffController.cs** bao gồm các action:

#### Dashboard
- `GET /Staff/Dashboard`: Hiển thị trang chủ quản lý

#### Quản lý sản phẩm
- `GET /Staff/Products`: Danh sách sản phẩm (có phân trang, tìm kiếm, lọc)
- `GET /Staff/CreateProduct`: Form thêm sản phẩm mới
- `POST /Staff/CreateProduct`: Xử lý thêm sản phẩm
- `GET /Staff/EditProduct/{id}`: Form sửa sản phẩm
- `POST /Staff/EditProduct`: Xử lý cập nhật sản phẩm
- `POST /Staff/DeleteProduct/{id}`: Xóa/vô hiệu hóa sản phẩm

#### Quản lý đơn hàng
- `GET /Staff/Orders`: Danh sách đơn hàng (có phân trang, tìm kiếm, lọc)
- `GET /Staff/OrderDetail/{id}`: Chi tiết đơn hàng
- `POST /Staff/UpdateOrderStatus`: Cập nhật trạng thái đơn hàng (AJAX)

#### Bán hàng trực tiếp
- `GET /Staff/DirectSale`: Giao diện bán hàng tại quầy
- `POST /Staff/CreateDirectSaleOrder`: Tạo đơn hàng trực tiếp (AJAX)
- `GET /Staff/SearchCustomer`: Tìm kiếm khách hàng theo SĐT (AJAX)

#### Thống kê
- `GET /Staff/SalesReport`: Báo cáo doanh số với biểu đồ

### 3. Views

Tất cả views được tạo trong `Views/Staff/`:
- `Dashboard.cshtml`: Trang chủ quản lý
- `Products.cshtml`: Danh sách sản phẩm
- `CreateProduct.cshtml`: Form thêm sản phẩm
- `EditProduct.cshtml`: Form sửa sản phẩm
- `Orders.cshtml`: Danh sách đơn hàng
- `OrderDetail.cshtml`: Chi tiết đơn hàng
- `DirectSale.cshtml`: Giao diện bán hàng trực tiếp
- `SalesReport.cshtml`: Báo cáo thống kê

## Cách Sử Dụng

### 1. Đăng Nhập

Nhân viên cần đăng nhập với tài khoản có Role = "Staff" hoặc "Admin".

Để tạo tài khoản Staff, cần:
1. Tạo Employee trong bảng Employees
2. Tạo User với Role = "Staff" và EmployeeId tương ứng

### 2. Dashboard

Sau khi đăng nhập, truy cập `/Staff/Dashboard` để xem:
- Doanh thu hôm nay và tháng này
- Số đơn hàng
- Đơn hàng chờ xử lý
- Sản phẩm sắp hết hàng
- Top sản phẩm bán chạy

### 3. Quản Lý Sản Phẩm

#### Xem danh sách
- Truy cập `/Staff/Products`
- Có thể tìm kiếm theo tên/mã sản phẩm
- Lọc theo danh mục
- Lọc theo tồn kho (sắp hết, hết hàng)

#### Thêm sản phẩm mới
1. Click "Thêm Sản Phẩm Mới"
2. Điền đầy đủ thông tin:
   - Mã sản phẩm (bắt buộc, duy nhất)
   - Tên sản phẩm (bắt buộc)
   - Danh mục (bắt buộc)
   - Giá bán (bắt buộc)
   - Số lượng tồn kho (bắt buộc)
   - Các thông tin khác (tùy chọn)
3. Click "Lưu Sản Phẩm"

#### Sửa sản phẩm
1. Click icon "Sửa" ở sản phẩm cần chỉnh sửa
2. Cập nhật thông tin
3. Click "Cập Nhật Sản Phẩm"

#### Xóa sản phẩm
- Click icon "Xóa"
- Nếu sản phẩm đã có trong đơn hàng, hệ thống sẽ vô hiệu hóa thay vì xóa

### 4. Quản Lý Đơn Hàng

#### Xem danh sách
- Truy cập `/Staff/Orders`
- Tìm kiếm theo mã đơn, tên khách hàng, SĐT
- Lọc theo trạng thái đơn hàng
- Lọc theo trạng thái thanh toán
- Lọc theo khoảng thời gian

#### Xem chi tiết & cập nhật
1. Click icon "Xem" ở đơn hàng
2. Xem đầy đủ thông tin đơn hàng
3. Cập nhật trạng thái:
   - Chọn trạng thái đơn hàng mới
   - Chọn trạng thái thanh toán
   - Thêm ghi chú (nếu cần)
   - Click "Cập Nhật"

### 5. Bán Hàng Trực Tiếp

Truy cập `/Staff/DirectSale` để bán hàng tại quầy:

#### Quy trình bán hàng
1. **Tìm kiếm sản phẩm**: Nhập tên hoặc chọn danh mục
2. **Thêm vào giỏ**: Click vào sản phẩm để thêm vào giỏ hàng
3. **Điều chỉnh số lượng**: Tăng/giảm số lượng trong giỏ hàng
4. **Nhập thông tin khách hàng**:
   - Nhập số điện thoại và click tìm kiếm (nếu là khách cũ)
   - Hoặc nhập thông tin mới
5. **Chọn phương thức thanh toán**: Tiền mặt, Chuyển khoản, Thẻ
6. **Áp dụng giảm giá** (nếu có)
7. **Click "Thanh Toán"** để hoàn tất

Hệ thống sẽ:
- Tự động tạo đơn hàng với OrderType = "Trực tiếp"
- Cập nhật tồn kho
- Đánh dấu đơn hàng là "Đã hoàn thành" và "Đã thanh toán"

### 6. Thống Kê Doanh Số

Truy cập `/Staff/SalesReport` để xem báo cáo:

#### Bộ lọc
- Chọn khoảng thời gian (từ ngày - đến ngày)
- Chọn loại báo cáo (theo ngày, tuần, tháng)

#### Thông tin hiển thị
- **Tổng quan**: Doanh thu, lợi nhuận, số đơn hàng, giá trị TB/đơn
- **Biểu đồ doanh thu**: Theo thời gian
- **Top sản phẩm**: Sản phẩm bán chạy nhất
- **Doanh thu theo danh mục**: Biểu đồ tròn
- **Phương thức thanh toán**: Phân tích theo phương thức

## Phân Quyền

Hệ thống kiểm tra quyền truy cập bằng cách:
- Kiểm tra Role = "Staff" hoặc "Admin"
- Lấy EmployeeId từ Claims để ghi nhận nhân viên xử lý

Nếu không có quyền, người dùng sẽ được chuyển về trang đăng nhập.

## Tính Năng Nổi Bật

### 1. Quản lý tồn kho thông minh
- Cảnh báo sản phẩm sắp hết hàng
- Tự động cập nhật tồn kho khi bán hàng
- Kiểm tra tồn kho trước khi tạo đơn

### 2. Bán hàng trực tiếp linh hoạt
- Giao diện POS thân thiện
- Tìm kiếm nhanh sản phẩm
- Tích hợp thông tin khách hàng cũ
- Hỗ trợ nhiều phương thức thanh toán

### 3. Thống kê chi tiết
- Biểu đồ trực quan với Chart.js
- Phân tích đa chiều (sản phẩm, danh mục, thanh toán)
- Tính toán lợi nhuận tự động

### 4. Quản lý đơn hàng hiệu quả
- Cập nhật trạng thái nhanh chóng
- Phân loại đơn online và trực tiếp
- Lịch sử đầy đủ

## Lưu Ý Kỹ Thuật

### Database
- Sử dụng Entity Framework Core
- Tất cả thao tác đều async/await
- Transaction tự động khi tạo đơn hàng

### Security
- Kiểm tra quyền truy cập ở mọi action
- Sử dụng AntiForgeryToken cho POST requests
- Validate dữ liệu đầu vào

### Performance
- Phân trang cho danh sách lớn
- Eager loading với Include() để tránh N+1 query
- Index trên các trường tìm kiếm

## Mở Rộng Trong Tương Lai

Có thể bổ sung:
1. **In hóa đơn**: Tích hợp PDF Service để in hóa đơn
2. **Quản lý ca làm việc**: Theo dõi ca làm việc của nhân viên
3. **Quản lý khách hàng**: CRUD khách hàng, lịch sử mua hàng
4. **Quản lý nhà cung cấp**: Nhập hàng, công nợ
5. **Báo cáo nâng cao**: Xuất Excel, so sánh theo kỳ
6. **Thông báo realtime**: SignalR cho đơn hàng mới
7. **Mobile responsive**: Tối ưu cho tablet/mobile

## Hỗ Trợ

Nếu gặp vấn đề, kiểm tra:
1. Connection string trong `appsettings.json`
2. Quyền truy cập database
3. Role và EmployeeId của user
4. Console log trong browser (F12) cho lỗi JavaScript

---

**Phiên bản**: 1.0  
**Ngày tạo**: 22/10/2025
