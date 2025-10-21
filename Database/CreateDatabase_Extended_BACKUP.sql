-- =============================================
-- Mộc Vị Store Database Schema - EXTENDED VERSION
-- Hỗ trợ bán hàng trực tiếp + Quản lý khách hàng
-- =============================================

USE master;
GO

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'MocViStoreDB')
BEGIN
    CREATE DATABASE MocViStoreDB;
END
GO

USE MocViStoreDB;
GO

-- =============================================
-- Table: Categories (Danh mục sản phẩm)
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Categories')
BEGIN
    CREATE TABLE Categories (
        CategoryId INT PRIMARY KEY IDENTITY(1,1),
        CategoryName NVARCHAR(100) NOT NULL,
        Description NVARCHAR(500),
        ImageUrl NVARCHAR(255),
        DisplayOrder INT DEFAULT 0,
        IsActive BIT DEFAULT 1,
        CreatedDate DATETIME DEFAULT GETDATE(),
        UpdatedDate DATETIME
    );
END
GO

-- =============================================
-- Table: Products (Sản phẩm)
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Products')
BEGIN
    CREATE TABLE Products (
        ProductId INT PRIMARY KEY IDENTITY(1,1),
        ProductCode NVARCHAR(50) UNIQUE NOT NULL, -- Mã sản phẩm để quét barcode
        ProductName NVARCHAR(200) NOT NULL,
        CategoryId INT NOT NULL,
        Description NVARCHAR(MAX),
        ShortDescription NVARCHAR(500),
        Price DECIMAL(18,2) NOT NULL,
        OriginalPrice DECIMAL(18,2),
        CostPrice DECIMAL(18,2), -- Giá vốn
        DiscountPercent INT DEFAULT 0,
        StockQuantity INT DEFAULT 0,
        MinStockLevel INT DEFAULT 10, -- Mức tồn kho tối thiểu
        Unit NVARCHAR(50) DEFAULT N'gói',
        Weight NVARCHAR(50),
        ImageUrl NVARCHAR(255),
        ImageGallery NVARCHAR(MAX),
        IsActive BIT DEFAULT 1,
        IsFeatured BIT DEFAULT 0,
        IsNew BIT DEFAULT 0,
        ViewCount INT DEFAULT 0,
        SoldCount INT DEFAULT 0,
        Rating DECIMAL(3,2) DEFAULT 0,
        CreatedDate DATETIME DEFAULT GETDATE(),
        UpdatedDate DATETIME,
        FOREIGN KEY (CategoryId) REFERENCES Categories(CategoryId)
    );
END
GO

-- =============================================
-- Table: Suppliers (Nhà cung cấp)
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Suppliers')
BEGIN
    CREATE TABLE Suppliers (
        SupplierId INT PRIMARY KEY IDENTITY(1,1),
        SupplierCode NVARCHAR(50) UNIQUE NOT NULL,
        SupplierName NVARCHAR(200) NOT NULL,
        ContactPerson NVARCHAR(100),
        PhoneNumber NVARCHAR(20),
        Email NVARCHAR(100),
        Address NVARCHAR(255),
        City NVARCHAR(100),
        District NVARCHAR(100),
        TaxCode NVARCHAR(50),
        BankAccount NVARCHAR(50),
        BankName NVARCHAR(100),
        Notes NVARCHAR(500),
        IsActive BIT DEFAULT 1,
        CreatedDate DATETIME DEFAULT GETDATE(),
        UpdatedDate DATETIME
    );
END
GO

-- =============================================
-- Table: PurchaseOrders (Đơn nhập hàng)
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'PurchaseOrders')
BEGIN
    CREATE TABLE PurchaseOrders (
        PurchaseOrderId INT PRIMARY KEY IDENTITY(1,1),
        PurchaseOrderCode NVARCHAR(50) UNIQUE NOT NULL,
        SupplierId INT NOT NULL,
        EmployeeId INT, -- Nhân viên nhập hàng
        TotalAmount DECIMAL(18,2) NOT NULL,
        PaidAmount DECIMAL(18,2) DEFAULT 0,
        RemainingAmount DECIMAL(18,2) DEFAULT 0,
        Status NVARCHAR(50) DEFAULT N'Chờ duyệt', -- Chờ duyệt, Đã duyệt, Đã nhập kho, Đã hủy
        Notes NVARCHAR(500),
        OrderDate DATETIME DEFAULT GETDATE(),
        ReceivedDate DATETIME,
        CreatedDate DATETIME DEFAULT GETDATE(),
        UpdatedDate DATETIME,
        FOREIGN KEY (SupplierId) REFERENCES Suppliers(SupplierId)
    );
END
GO

-- =============================================
-- Table: PurchaseOrderDetails (Chi tiết đơn nhập)
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'PurchaseOrderDetails')
BEGIN
    CREATE TABLE PurchaseOrderDetails (
        PurchaseOrderDetailId INT PRIMARY KEY IDENTITY(1,1),
        PurchaseOrderId INT NOT NULL,
        ProductId INT NOT NULL,
        Quantity INT NOT NULL,
        UnitPrice DECIMAL(18,2) NOT NULL,
        TotalPrice DECIMAL(18,2) NOT NULL,
        FOREIGN KEY (PurchaseOrderId) REFERENCES PurchaseOrders(PurchaseOrderId),
        FOREIGN KEY (ProductId) REFERENCES Products(ProductId)
    );
END
GO

-- =============================================
-- Table: InventoryTransactions (Lịch sử xuất nhập kho)
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'InventoryTransactions')
BEGIN
    CREATE TABLE InventoryTransactions (
        TransactionId INT PRIMARY KEY IDENTITY(1,1),
        ProductId INT NOT NULL,
        TransactionType NVARCHAR(50) NOT NULL, -- Nhập kho, Xuất kho, Kiểm kê, Hủy hàng
        Quantity INT NOT NULL, -- Số lượng (+ nhập, - xuất)
        ReferenceType NVARCHAR(50), -- PurchaseOrder, Order, Adjustment
        ReferenceId INT, -- ID của đơn hàng/phiếu nhập
        Notes NVARCHAR(500),
        EmployeeId INT,
        CreatedDate DATETIME DEFAULT GETDATE(),
        FOREIGN KEY (ProductId) REFERENCES Products(ProductId)
    );
END
GO

-- =============================================
-- Table: Customers (Khách hàng - tách riêng)
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Customers')
BEGIN
    CREATE TABLE Customers (
        CustomerId INT PRIMARY KEY IDENTITY(1,1),
        CustomerCode NVARCHAR(50) UNIQUE, -- Mã khách hàng
        FullName NVARCHAR(100) NOT NULL,
        PhoneNumber NVARCHAR(20) NOT NULL,
        Email NVARCHAR(100),
        DateOfBirth DATE,
        Gender NVARCHAR(10), -- Nam, Nữ, Khác
        Address NVARCHAR(255),
        City NVARCHAR(100),
        District NVARCHAR(100),
        Ward NVARCHAR(100),
        CustomerType NVARCHAR(50) DEFAULT N'Thường', -- Thường, VIP, Đại lý
        TotalPurchased DECIMAL(18,2) DEFAULT 0, -- Tổng tiền đã mua
        TotalOrders INT DEFAULT 0, -- Tổng số đơn hàng
        LoyaltyPoints INT DEFAULT 0, -- Điểm tích lũy
        Notes NVARCHAR(500),
        IsActive BIT DEFAULT 1,
        CreatedDate DATETIME DEFAULT GETDATE(),
        UpdatedDate DATETIME,
        LastPurchaseDate DATETIME
    );
END
GO

-- =============================================
-- Table: Employees (Nhân viên)
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Employees')
BEGIN
    CREATE TABLE Employees (
        EmployeeId INT PRIMARY KEY IDENTITY(1,1),
        EmployeeCode NVARCHAR(50) UNIQUE NOT NULL,
        FullName NVARCHAR(100) NOT NULL,
        PhoneNumber NVARCHAR(20),
        Email NVARCHAR(100),
        DateOfBirth DATE,
        Gender NVARCHAR(10),
        Address NVARCHAR(255),
        Position NVARCHAR(100), -- Quản lý, Thu ngân, Nhân viên bán hàng, Kho
        Department NVARCHAR(100), -- Bán hàng, Kho, Kế toán
        Salary DECIMAL(18,2),
        HireDate DATE,
        IdentityCard NVARCHAR(20),
        BankAccount NVARCHAR(50),
        BankName NVARCHAR(100),
        IsActive BIT DEFAULT 1,
        CreatedDate DATETIME DEFAULT GETDATE(),
        UpdatedDate DATETIME
    );
END
GO

-- =============================================
-- Table: Users (Tài khoản đăng nhập - liên kết Employee)
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Users')
BEGIN
    CREATE TABLE Users (
        UserId INT PRIMARY KEY IDENTITY(1,1),
        Email NVARCHAR(100) NOT NULL UNIQUE,
        PasswordHash NVARCHAR(255) NOT NULL,
        FullName NVARCHAR(100) NOT NULL,
        PhoneNumber NVARCHAR(20),
        Role NVARCHAR(20) DEFAULT 'Customer', -- Admin, Manager, Cashier, Staff, Customer
        EmployeeId INT, -- Liên kết với nhân viên nếu là tài khoản nội bộ
        CustomerId INT, -- Liên kết với khách hàng nếu là tài khoản online
        IsActive BIT DEFAULT 1,
        CreatedDate DATETIME DEFAULT GETDATE(),
        LastLoginDate DATETIME,
        FOREIGN KEY (EmployeeId) REFERENCES Employees(EmployeeId),
        FOREIGN KEY (CustomerId) REFERENCES Customers(CustomerId)
    );
END
GO

-- =============================================
-- Table: Orders (Đơn hàng - cả online và offline)
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Orders')
BEGIN
    CREATE TABLE Orders (
        OrderId INT PRIMARY KEY IDENTITY(1,1),
        OrderCode NVARCHAR(50) UNIQUE NOT NULL,
        CustomerId INT, -- Khách hàng
        EmployeeId INT, -- Nhân viên xử lý (nếu bán trực tiếp)
        OrderType NVARCHAR(50) DEFAULT N'Online', -- Online, POS (bán trực tiếp)
        CustomerName NVARCHAR(100) NOT NULL,
        CustomerEmail NVARCHAR(100),
        CustomerPhone NVARCHAR(20) NOT NULL,
        ShippingAddress NVARCHAR(255),
        City NVARCHAR(100),
        District NVARCHAR(100),
        Ward NVARCHAR(100),
        TotalAmount DECIMAL(18,2) NOT NULL,
        ShippingFee DECIMAL(18,2) DEFAULT 0,
        DiscountAmount DECIMAL(18,2) DEFAULT 0,
        VoucherCode NVARCHAR(50),
        LoyaltyPointsUsed INT DEFAULT 0,
        LoyaltyPointsEarned INT DEFAULT 0,
        FinalAmount DECIMAL(18,2) NOT NULL,
        PaymentMethod NVARCHAR(50), -- Tiền mặt, Chuyển khoản, Thẻ, Momo, COD
        PaymentStatus NVARCHAR(50) DEFAULT N'Chưa thanh toán',
        OrderStatus NVARCHAR(50) DEFAULT N'Chờ xác nhận',
        Note NVARCHAR(500),
        CreatedDate DATETIME DEFAULT GETDATE(),
        UpdatedDate DATETIME,
        CompletedDate DATETIME,
        FOREIGN KEY (CustomerId) REFERENCES Customers(CustomerId),
        FOREIGN KEY (EmployeeId) REFERENCES Employees(EmployeeId)
    );
END
GO

-- =============================================
-- Table: OrderDetails (Chi tiết đơn hàng)
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'OrderDetails')
BEGIN
    CREATE TABLE OrderDetails (
        OrderDetailId INT PRIMARY KEY IDENTITY(1,1),
        OrderId INT NOT NULL,
        ProductId INT NOT NULL,
        ProductName NVARCHAR(200) NOT NULL,
        Price DECIMAL(18,2) NOT NULL,
        Quantity INT NOT NULL,
        DiscountPercent INT DEFAULT 0,
        TotalPrice DECIMAL(18,2) NOT NULL,
        FOREIGN KEY (OrderId) REFERENCES Orders(OrderId),
        FOREIGN KEY (ProductId) REFERENCES Products(ProductId)
    );
END
GO

-- =============================================
-- Table: Payments (Thanh toán - theo dõi chi tiết)
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Payments')
BEGIN
    CREATE TABLE Payments (
        PaymentId INT PRIMARY KEY IDENTITY(1,1),
        OrderId INT NOT NULL,
        PaymentMethod NVARCHAR(50) NOT NULL,
        Amount DECIMAL(18,2) NOT NULL,
        TransactionCode NVARCHAR(100),
        Status NVARCHAR(50) DEFAULT N'Thành công', -- Thành công, Thất bại, Đang xử lý
        Notes NVARCHAR(500),
        EmployeeId INT, -- Nhân viên thu tiền (nếu bán trực tiếp)
        PaymentDate DATETIME DEFAULT GETDATE(),
        FOREIGN KEY (OrderId) REFERENCES Orders(OrderId),
        FOREIGN KEY (EmployeeId) REFERENCES Employees(EmployeeId)
    );
END
GO

-- =============================================
-- Table: Vouchers (Mã giảm giá)
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Vouchers')
BEGIN
    CREATE TABLE Vouchers (
        VoucherId INT PRIMARY KEY IDENTITY(1,1),
        VoucherCode NVARCHAR(50) UNIQUE NOT NULL,
        VoucherName NVARCHAR(200),
        DiscountType NVARCHAR(20), -- Percent, Fixed
        DiscountValue DECIMAL(18,2) NOT NULL,
        MinOrderAmount DECIMAL(18,2) DEFAULT 0,
        MaxDiscountAmount DECIMAL(18,2),
        UsageLimit INT, -- Số lần sử dụng tối đa
        UsedCount INT DEFAULT 0,
        ValidFrom DATETIME,
        ValidTo DATETIME,
        IsActive BIT DEFAULT 1,
        CreatedDate DATETIME DEFAULT GETDATE()
    );
END
GO

-- =============================================
-- Table: LoyaltyPointsHistory (Lịch sử điểm tích lũy)
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'LoyaltyPointsHistory')
BEGIN
    CREATE TABLE LoyaltyPointsHistory (
        HistoryId INT PRIMARY KEY IDENTITY(1,1),
        CustomerId INT NOT NULL,
        OrderId INT,
        Points INT NOT NULL, -- + tích điểm, - tiêu điểm
        TransactionType NVARCHAR(50), -- Earned, Redeemed, Adjusted
        Description NVARCHAR(255),
        CreatedDate DATETIME DEFAULT GETDATE(),
        FOREIGN KEY (CustomerId) REFERENCES Customers(CustomerId),
        FOREIGN KEY (OrderId) REFERENCES Orders(OrderId)
    );
END
GO

-- =============================================
-- Table: Shifts (Ca làm việc)
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Shifts')
BEGIN
    CREATE TABLE Shifts (
        ShiftId INT PRIMARY KEY IDENTITY(1,1),
        ShiftCode NVARCHAR(50) UNIQUE NOT NULL,
        EmployeeId INT NOT NULL,
        StartTime DATETIME NOT NULL,
        EndTime DATETIME,
        OpeningCash DECIMAL(18,2) DEFAULT 0, -- Tiền đầu ca
        ClosingCash DECIMAL(18,2), -- Tiền cuối ca
        TotalSales DECIMAL(18,2) DEFAULT 0, -- Tổng doanh thu
        TotalOrders INT DEFAULT 0, -- Tổng số đơn
        Status NVARCHAR(50) DEFAULT N'Đang mở', -- Đang mở, Đã đóng
        Notes NVARCHAR(500),
        CreatedDate DATETIME DEFAULT GETDATE(),
        FOREIGN KEY (EmployeeId) REFERENCES Employees(EmployeeId)
    );
END
GO

-- =============================================
-- Table: Cart (Giỏ hàng)
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Cart')
BEGIN
    CREATE TABLE Cart (
        CartId INT PRIMARY KEY IDENTITY(1,1),
        CustomerId INT,
        SessionId NVARCHAR(100),
        ProductId INT NOT NULL,
        Quantity INT NOT NULL DEFAULT 1,
        CreatedDate DATETIME DEFAULT GETDATE(),
        FOREIGN KEY (CustomerId) REFERENCES Customers(CustomerId),
        FOREIGN KEY (ProductId) REFERENCES Products(ProductId)
    );
END
GO

-- =============================================
-- Table: Reviews (Đánh giá sản phẩm)
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Reviews')
BEGIN
    CREATE TABLE Reviews (
        ReviewId INT PRIMARY KEY IDENTITY(1,1),
        ProductId INT NOT NULL,
        CustomerId INT,
        CustomerName NVARCHAR(100) NOT NULL,
        Rating INT NOT NULL CHECK (Rating >= 1 AND Rating <= 5),
        Comment NVARCHAR(1000),
        IsApproved BIT DEFAULT 0,
        CreatedDate DATETIME DEFAULT GETDATE(),
        FOREIGN KEY (ProductId) REFERENCES Products(ProductId),
        FOREIGN KEY (CustomerId) REFERENCES Customers(CustomerId)
    );
END
GO

-- =============================================
-- Table: Blogs (Bài viết blog)
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Blogs')
BEGIN
    CREATE TABLE Blogs (
        BlogId INT PRIMARY KEY IDENTITY(1,1),
        Title NVARCHAR(255) NOT NULL,
        Slug NVARCHAR(255) UNIQUE NOT NULL,
        Content NVARCHAR(MAX) NOT NULL,
        ShortDescription NVARCHAR(500),
        ImageUrl NVARCHAR(255),
        AuthorId INT,
        AuthorName NVARCHAR(100),
        ViewCount INT DEFAULT 0,
        IsPublished BIT DEFAULT 1,
        PublishedDate DATETIME,
        CreatedDate DATETIME DEFAULT GETDATE(),
        UpdatedDate DATETIME,
        FOREIGN KEY (AuthorId) REFERENCES Users(UserId)
    );
END
GO

-- =============================================
-- Table: BlogComments (Bình luận blog)
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'BlogComments')
BEGIN
    CREATE TABLE BlogComments (
        CommentId INT PRIMARY KEY IDENTITY(1,1),
        BlogId INT NOT NULL,
        CustomerId INT,
        CustomerName NVARCHAR(100) NOT NULL,
        CustomerEmail NVARCHAR(100),
        Comment NVARCHAR(1000) NOT NULL,
        IsApproved BIT DEFAULT 0,
        CreatedDate DATETIME DEFAULT GETDATE(),
        FOREIGN KEY (BlogId) REFERENCES Blogs(BlogId),
        FOREIGN KEY (CustomerId) REFERENCES Customers(CustomerId)
    );
END
GO

-- =============================================
-- Table: ContactMessages (Tin nhắn liên hệ)
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ContactMessages')
BEGIN
    CREATE TABLE ContactMessages (
        MessageId INT PRIMARY KEY IDENTITY(1,1),
        FullName NVARCHAR(100) NOT NULL,
        Email NVARCHAR(100) NOT NULL,
        PhoneNumber NVARCHAR(20),
        Subject NVARCHAR(255),
        Message NVARCHAR(MAX) NOT NULL,
        IsRead BIT DEFAULT 0,
        IsReplied BIT DEFAULT 0,
        CreatedDate DATETIME DEFAULT GETDATE()
    );
END
GO

-- =============================================
-- Table: Expenses (Chi phí)
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Expenses')
BEGIN
    CREATE TABLE Expenses (
        ExpenseId INT PRIMARY KEY IDENTITY(1,1),
        ExpenseCode NVARCHAR(50) UNIQUE NOT NULL,
        ExpenseType NVARCHAR(100), -- Tiền điện, Tiền nước, Lương, Vận chuyển, Khác
        Amount DECIMAL(18,2) NOT NULL,
        Description NVARCHAR(500),
        EmployeeId INT,
        ExpenseDate DATE NOT NULL,
        Status NVARCHAR(50) DEFAULT N'Đã chi', -- Đã chi, Chờ duyệt
        CreatedDate DATETIME DEFAULT GETDATE(),
        FOREIGN KEY (EmployeeId) REFERENCES Employees(EmployeeId)
    );
END
GO

-- =============================================
-- Table: Settings (Cấu hình website)
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Settings')
BEGIN
    CREATE TABLE Settings (
        SettingId INT PRIMARY KEY IDENTITY(1,1),
        SettingKey NVARCHAR(100) UNIQUE NOT NULL,
        SettingValue NVARCHAR(MAX),
        Description NVARCHAR(255),
        UpdatedDate DATETIME DEFAULT GETDATE()
    );
END
GO

PRINT 'Extended database schema created successfully!';
PRINT 'Hỗ trợ: Bán hàng trực tiếp (POS), Quản lý khách hàng, Quản lý kho, Nhân viên, Ca làm việc';
GO
