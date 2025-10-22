using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Exe_Demo.Data;
using Exe_Demo.Models;
using Exe_Demo.Models.ViewModels;
using System.Security.Claims;

namespace Exe_Demo.Controllers
{
    public class StaffController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StaffController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Kiểm tra quyền Staff
        private bool IsStaff()
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            return role == "Staff" || role == "Admin";
        }

        private int? GetEmployeeId()
        {
            var employeeIdClaim = User.FindFirst("EmployeeId")?.Value;
            if (int.TryParse(employeeIdClaim, out int employeeId))
            {
                return employeeId;
            }
            return null;
        }

        // ==================== DASHBOARD ====================
        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            if (!IsStaff())
            {
                return RedirectToAction("Login", "Auth");
            }

            var employeeId = GetEmployeeId();
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.EmployeeId == employeeId);

            var today = DateTime.Today;
            var startOfMonth = new DateTime(today.Year, today.Month, 1);

            var model = new StaffDashboardViewModel
            {
                EmployeeName = employee?.FullName ?? "Staff",
                EmployeeCode = employee?.EmployeeCode ?? "",
                Position = employee?.Position ?? "",

                // Doanh thu hôm nay
                TodayRevenue = await _context.Orders
                    .Where(o => o.CreatedDate.HasValue && 
                                o.CreatedDate.Value.Date == today &&
                                o.PaymentStatus == "Đã thanh toán")
                    .SumAsync(o => o.FinalAmount),

                // Doanh thu tháng này
                MonthRevenue = await _context.Orders
                    .Where(o => o.CreatedDate.HasValue && 
                                o.CreatedDate.Value >= startOfMonth &&
                                o.PaymentStatus == "Đã thanh toán")
                    .SumAsync(o => o.FinalAmount),

                // Đơn hàng hôm nay
                TodayOrders = await _context.Orders
                    .CountAsync(o => o.CreatedDate.HasValue && o.CreatedDate.Value.Date == today),

                // Đơn hàng tháng này
                MonthOrders = await _context.Orders
                    .CountAsync(o => o.CreatedDate.HasValue && o.CreatedDate.Value >= startOfMonth),

                // Đơn hàng chờ xử lý
                PendingOrders = await _context.Orders
                    .CountAsync(o => o.OrderStatus == "Chờ xác nhận" || o.OrderStatus == "Đang xử lý"),

                // Sản phẩm sắp hết hàng
                LowStockProducts = await _context.Products
                    .CountAsync(p => p.StockQuantity <= p.MinStockLevel),

                // Đơn hàng gần đây
                RecentOrders = await _context.Orders
                    .Include(o => o.Customer)
                    .OrderByDescending(o => o.CreatedDate)
                    .Take(10)
                    .ToListAsync(),

                // Top sản phẩm bán chạy
                TopSellingProducts = await _context.OrderDetails
                    .Where(od => od.Order.CreatedDate.HasValue && od.Order.CreatedDate.Value >= startOfMonth)
                    .GroupBy(od => new { od.ProductId, od.Product.ProductName, od.Product.ImageUrl })
                    .Select(g => new ProductSalesViewModel
                    {
                        ProductId = g.Key.ProductId,
                        ProductName = g.Key.ProductName,
                        ImageUrl = g.Key.ImageUrl,
                        TotalSold = g.Sum(od => od.Quantity),
                        Revenue = g.Sum(od => od.TotalPrice)
                    })
                    .OrderByDescending(p => p.TotalSold)
                    .Take(5)
                    .ToListAsync()
            };

            return View(model);
        }

        // ==================== QUẢN LÝ SẢN PHẨM ====================
        [HttpGet]
        public async Task<IActionResult> Products(int page = 1, string? search = null, int? category = null, string? stock = null)
        {
            if (!IsStaff())
            {
                return RedirectToAction("Login", "Auth");
            }

            int pageSize = 20;
            var query = _context.Products.Include(p => p.Category).AsQueryable();

            // Tìm kiếm
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(p => p.ProductName.Contains(search) || p.ProductCode.Contains(search));
            }

            // Lọc theo danh mục
            if (category.HasValue)
            {
                query = query.Where(p => p.CategoryId == category.Value);
            }

            // Lọc theo tồn kho
            if (!string.IsNullOrEmpty(stock))
            {
                if (stock == "low")
                {
                    query = query.Where(p => p.StockQuantity <= p.MinStockLevel && p.StockQuantity > 0);
                }
                else if (stock == "out")
                {
                    query = query.Where(p => p.StockQuantity == 0);
                }
            }

            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var products = await query
                .OrderByDescending(p => p.CreatedDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var model = new ProductManagementViewModel
            {
                Products = products,
                Categories = await _context.Categories.Where(c => c.IsActive == true).ToListAsync(),
                CurrentPage = page,
                TotalPages = totalPages,
                PageSize = pageSize,
                SearchTerm = search,
                CategoryFilter = category,
                StockFilter = stock
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> CreateProduct()
        {
            if (!IsStaff())
            {
                return RedirectToAction("Login", "Auth");
            }

            var model = new ProductFormViewModel
            {
                Categories = await _context.Categories.Where(c => c.IsActive == true).ToListAsync(),
                IsActive = true,
                MinStockLevel = 10,
                Unit = "gói"
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProduct(ProductFormViewModel model)
        {
            if (!IsStaff())
            {
                return RedirectToAction("Login", "Auth");
            }

            if (ModelState.IsValid)
            {
                // Kiểm tra mã sản phẩm đã tồn tại
                if (await _context.Products.AnyAsync(p => p.ProductCode == model.ProductCode))
                {
                    ModelState.AddModelError("ProductCode", "Mã sản phẩm đã tồn tại");
                    model.Categories = await _context.Categories.Where(c => c.IsActive == true).ToListAsync();
                    return View(model);
                }

                var product = new Product
                {
                    ProductCode = model.ProductCode,
                    ProductName = model.ProductName,
                    CategoryId = model.CategoryId,
                    Description = model.Description,
                    ShortDescription = model.ShortDescription,
                    Price = model.Price,
                    OriginalPrice = model.OriginalPrice,
                    CostPrice = model.CostPrice,
                    DiscountPercent = model.DiscountPercent,
                    StockQuantity = model.StockQuantity,
                    MinStockLevel = model.MinStockLevel ?? 10,
                    Unit = model.Unit ?? "gói",
                    Weight = model.Weight,
                    ImageUrl = model.ImageUrl,
                    IsActive = model.IsActive,
                    IsFeatured = model.IsFeatured,
                    IsNew = model.IsNew,
                    ViewCount = 0,
                    SoldCount = 0,
                    Rating = 0,
                    CreatedDate = DateTime.Now
                };

                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Thêm sản phẩm thành công!";
                return RedirectToAction(nameof(Products));
            }

            model.Categories = await _context.Categories.Where(c => c.IsActive == true).ToListAsync();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditProduct(int id)
        {
            if (!IsStaff())
            {
                return RedirectToAction("Login", "Auth");
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            var model = new ProductFormViewModel
            {
                ProductId = product.ProductId,
                ProductCode = product.ProductCode,
                ProductName = product.ProductName,
                CategoryId = product.CategoryId,
                Description = product.Description,
                ShortDescription = product.ShortDescription,
                Price = product.Price,
                OriginalPrice = product.OriginalPrice,
                CostPrice = product.CostPrice,
                DiscountPercent = product.DiscountPercent,
                StockQuantity = product.StockQuantity ?? 0,
                MinStockLevel = product.MinStockLevel,
                Unit = product.Unit,
                Weight = product.Weight,
                ImageUrl = product.ImageUrl,
                IsActive = product.IsActive ?? true,
                IsFeatured = product.IsFeatured ?? false,
                IsNew = product.IsNew ?? false,
                Categories = await _context.Categories.Where(c => c.IsActive == true).ToListAsync()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProduct(ProductFormViewModel model)
        {
            if (!IsStaff())
            {
                return RedirectToAction("Login", "Auth");
            }

            if (ModelState.IsValid)
            {
                var product = await _context.Products.FindAsync(model.ProductId);
                if (product == null)
                {
                    return NotFound();
                }

                // Kiểm tra mã sản phẩm đã tồn tại (trừ sản phẩm hiện tại)
                if (await _context.Products.AnyAsync(p => p.ProductCode == model.ProductCode && p.ProductId != model.ProductId))
                {
                    ModelState.AddModelError("ProductCode", "Mã sản phẩm đã tồn tại");
                    model.Categories = await _context.Categories.Where(c => c.IsActive == true).ToListAsync();
                    return View(model);
                }

                product.ProductCode = model.ProductCode;
                product.ProductName = model.ProductName;
                product.CategoryId = model.CategoryId;
                product.Description = model.Description;
                product.ShortDescription = model.ShortDescription;
                product.Price = model.Price;
                product.OriginalPrice = model.OriginalPrice;
                product.CostPrice = model.CostPrice;
                product.DiscountPercent = model.DiscountPercent;
                product.StockQuantity = model.StockQuantity;
                product.MinStockLevel = model.MinStockLevel;
                product.Unit = model.Unit;
                product.Weight = model.Weight;
                product.ImageUrl = model.ImageUrl;
                product.IsActive = model.IsActive;
                product.IsFeatured = model.IsFeatured;
                product.IsNew = model.IsNew;
                product.UpdatedDate = DateTime.Now;

                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Cập nhật sản phẩm thành công!";
                return RedirectToAction(nameof(Products));
            }

            model.Categories = await _context.Categories.Where(c => c.IsActive == true).ToListAsync();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if (!IsStaff())
            {
                return Json(new { success = false, message = "Không có quyền truy cập" });
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return Json(new { success = false, message = "Không tìm thấy sản phẩm" });
            }

            // Kiểm tra xem sản phẩm có trong đơn hàng nào không
            var hasOrders = await _context.OrderDetails.AnyAsync(od => od.ProductId == id);
            if (hasOrders)
            {
                // Chỉ vô hiệu hóa thay vì xóa
                product.IsActive = false;
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Đã vô hiệu hóa sản phẩm" });
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Xóa sản phẩm thành công" });
        }

        // ==================== QUẢN LÝ ĐỚN HÀNG ====================
        [HttpGet]
        public async Task<IActionResult> Orders(int page = 1, string? search = null, string? status = null, 
            string? paymentStatus = null, DateTime? fromDate = null, DateTime? toDate = null)
        {
            if (!IsStaff())
            {
                return RedirectToAction("Login", "Auth");
            }

            int pageSize = 20;
            var query = _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Employee)
                .AsQueryable();

            // Tìm kiếm
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(o => o.OrderCode.Contains(search) || 
                                        o.CustomerName.Contains(search) || 
                                        o.CustomerPhone.Contains(search));
            }

            // Lọc theo trạng thái
            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(o => o.OrderStatus == status);
            }

            // Lọc theo trạng thái thanh toán
            if (!string.IsNullOrEmpty(paymentStatus))
            {
                query = query.Where(o => o.PaymentStatus == paymentStatus);
            }

            // Lọc theo ngày
            if (fromDate.HasValue)
            {
                query = query.Where(o => o.CreatedDate >= fromDate.Value);
            }
            if (toDate.HasValue)
            {
                var endDate = toDate.Value.AddDays(1);
                query = query.Where(o => o.CreatedDate < endDate);
            }

            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var orders = await query
                .OrderByDescending(o => o.CreatedDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var model = new OrderManagementViewModel
            {
                Orders = orders,
                CurrentPage = page,
                TotalPages = totalPages,
                PageSize = pageSize,
                SearchTerm = search,
                StatusFilter = status,
                PaymentStatusFilter = paymentStatus,
                FromDate = fromDate,
                ToDate = toDate
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> OrderDetail(int id)
        {
            if (!IsStaff())
            {
                return RedirectToAction("Login", "Auth");
            }

            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Employee)
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null)
            {
                return NotFound();
            }

            var orderDetails = await _context.OrderDetails
                .Include(od => od.Product)
                .Where(od => od.OrderId == id)
                .ToListAsync();

            var model = new OrderDetailViewModel
            {
                Order = order,
                OrderDetails = orderDetails,
                Customer = order.Customer,
                Employee = order.Employee
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateOrderStatus([FromBody] UpdateOrderStatusViewModel model)
        {
            if (!IsStaff())
            {
                return Json(new { success = false, message = "Không có quyền truy cập" });
            }

            var order = await _context.Orders.FindAsync(model.OrderId);
            if (order == null)
            {
                return Json(new { success = false, message = "Không tìm thấy đơn hàng" });
            }

            order.OrderStatus = model.OrderStatus;
            if (!string.IsNullOrEmpty(model.PaymentStatus))
            {
                order.PaymentStatus = model.PaymentStatus;
            }
            if (!string.IsNullOrEmpty(model.Note))
            {
                order.Note = model.Note;
            }
            order.UpdatedDate = DateTime.Now;

            if (model.OrderStatus == "Đã hoàn thành")
            {
                order.CompletedDate = DateTime.Now;
            }

            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Cập nhật trạng thái thành công" });
        }

        // ==================== BÁN HÀNG TRỰC TIẾP ====================
        [HttpGet]
        public async Task<IActionResult> DirectSale(string? search = null, int? category = null)
        {
            if (!IsStaff())
            {
                return RedirectToAction("Login", "Auth");
            }

            var query = _context.Products
                .Include(p => p.Category)
                .Where(p => p.IsActive == true && p.StockQuantity > 0)
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(p => p.ProductName.Contains(search) || p.ProductCode.Contains(search));
            }

            if (category.HasValue)
            {
                query = query.Where(p => p.CategoryId == category.Value);
            }

            var model = new DirectSaleViewModel
            {
                Products = await query.Take(50).ToListAsync(),
                Categories = await _context.Categories.Where(c => c.IsActive == true).ToListAsync(),
                SearchTerm = search,
                CategoryFilter = category
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDirectSaleOrder([FromBody] CreateDirectSaleOrderViewModel model)
        {
            if (!IsStaff())
            {
                return Json(new { success = false, message = "Không có quyền truy cập" });
            }

            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Dữ liệu không hợp lệ" });
            }

            var employeeId = GetEmployeeId();
            if (!employeeId.HasValue)
            {
                return Json(new { success = false, message = "Không tìm thấy thông tin nhân viên" });
            }

            // Tạo mã đơn hàng
            var orderCode = "ORD" + DateTime.Now.ToString("yyyyMMddHHmmss");

            // Tính tổng tiền
            decimal totalAmount = 0;
            foreach (var item in model.Items)
            {
                var product = await _context.Products.FindAsync(item.ProductId);
                if (product == null || product.StockQuantity < item.Quantity)
                {
                    return Json(new { success = false, message = $"Sản phẩm {product?.ProductName} không đủ hàng" });
                }

                var itemTotal = item.Price * item.Quantity;
                if (item.DiscountPercent > 0)
                {
                    itemTotal = itemTotal * (100 - item.DiscountPercent) / 100;
                }
                totalAmount += itemTotal;
            }

            var finalAmount = totalAmount - model.DiscountAmount;

            // Tạo đơn hàng
            var order = new Order
            {
                OrderCode = orderCode,
                CustomerId = model.CustomerId,
                EmployeeId = employeeId.Value,
                OrderType = "Trực tiếp",
                CustomerName = model.CustomerName,
                CustomerEmail = model.CustomerEmail,
                CustomerPhone = model.CustomerPhone,
                TotalAmount = totalAmount,
                DiscountAmount = model.DiscountAmount,
                FinalAmount = finalAmount,
                PaymentMethod = model.PaymentMethod,
                PaymentStatus = "Đã thanh toán",
                OrderStatus = "Đã hoàn thành",
                Note = model.Note,
                CreatedDate = DateTime.Now,
                CompletedDate = DateTime.Now
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Tạo chi tiết đơn hàng và cập nhật tồn kho
            foreach (var item in model.Items)
            {
                var product = await _context.Products.FindAsync(item.ProductId);
                
                var orderDetail = new OrderDetail
                {
                    OrderId = order.OrderId,
                    ProductId = item.ProductId,
                    ProductName = product!.ProductName,
                    Price = item.Price,
                    Quantity = item.Quantity,
                    DiscountPercent = item.DiscountPercent,
                    TotalPrice = item.Price * item.Quantity * (100 - item.DiscountPercent) / 100
                };

                _context.OrderDetails.Add(orderDetail);

                // Cập nhật tồn kho
                product.StockQuantity -= item.Quantity;
                product.SoldCount = (product.SoldCount ?? 0) + item.Quantity;
            }

            await _context.SaveChangesAsync();

            return Json(new { 
                success = true, 
                message = "Tạo đơn hàng thành công", 
                orderId = order.OrderId,
                orderCode = order.OrderCode
            });
        }

        [HttpGet]
        public async Task<IActionResult> SearchCustomer(string phone)
        {
            if (!IsStaff())
            {
                return Json(new { success = false, message = "Không có quyền truy cập" });
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.PhoneNumber == phone);

            if (customer == null)
            {
                return Json(new { success = false, message = "Không tìm thấy khách hàng" });
            }

            return Json(new { 
                success = true, 
                customer = new {
                    customerId = customer.CustomerId,
                    fullName = customer.FullName,
                    email = customer.Email,
                    phoneNumber = customer.PhoneNumber,
                    loyaltyPoints = customer.LoyaltyPoints
                }
            });
        }

        // ==================== THỐNG KÊ DOANH SỐ ====================
        [HttpGet]
        public async Task<IActionResult> SalesReport(DateTime? fromDate = null, DateTime? toDate = null, string reportType = "daily")
        {
            if (!IsStaff())
            {
                return RedirectToAction("Login", "Auth");
            }

            // Mặc định là tháng hiện tại
            if (!fromDate.HasValue)
            {
                fromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            }
            if (!toDate.HasValue)
            {
                toDate = DateTime.Now;
            }

            var orders = await _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .ThenInclude(p => p.Category)
                .Where(o => o.CreatedDate >= fromDate && o.CreatedDate <= toDate && o.PaymentStatus == "Đã thanh toán")
                .ToListAsync();

            var model = new SalesReportViewModel
            {
                FromDate = fromDate,
                ToDate = toDate,
                ReportType = reportType,
                TotalRevenue = orders.Sum(o => o.FinalAmount),
                TotalOrders = orders.Count,
                TotalProducts = orders.SelectMany(o => o.OrderDetails).Sum(od => od.Quantity)
            };

            // Tính tổng chi phí và lợi nhuận
            foreach (var order in orders)
            {
                foreach (var detail in order.OrderDetails)
                {
                    var costPrice = detail.Product?.CostPrice ?? 0;
                    model.TotalCost += costPrice * detail.Quantity;
                }
            }
            model.TotalProfit = model.TotalRevenue - model.TotalCost;
            model.AverageOrderValue = model.TotalOrders > 0 ? model.TotalRevenue / model.TotalOrders : 0;

            // Doanh thu theo ngày
            model.RevenueByDate = orders
                .GroupBy(o => o.CreatedDate!.Value.Date)
                .Select(g => new RevenueByDateViewModel
                {
                    Date = g.Key,
                    Revenue = g.Sum(o => o.FinalAmount),
                    OrderCount = g.Count()
                })
                .OrderBy(r => r.Date)
                .ToList();

            // Top sản phẩm bán chạy
            model.TopProducts = orders
                .SelectMany(o => o.OrderDetails)
                .GroupBy(od => new { od.ProductId, od.Product!.ProductCode, od.Product.ProductName, od.Product.ImageUrl, od.Product.CostPrice })
                .Select(g => new TopProductViewModel
                {
                    ProductId = g.Key.ProductId,
                    ProductCode = g.Key.ProductCode,
                    ProductName = g.Key.ProductName,
                    ImageUrl = g.Key.ImageUrl,
                    QuantitySold = g.Sum(od => od.Quantity),
                    Revenue = g.Sum(od => od.TotalPrice),
                    Profit = g.Sum(od => od.TotalPrice) - (g.Key.CostPrice ?? 0) * g.Sum(od => od.Quantity)
                })
                .OrderByDescending(p => p.QuantitySold)
                .Take(10)
                .ToList();

            // Doanh thu theo danh mục
            var categoryRevenue = orders
                .SelectMany(o => o.OrderDetails)
                .GroupBy(od => new { od.Product!.CategoryId, od.Product.Category!.CategoryName })
                .Select(g => new RevenueByCategoryViewModel
                {
                    CategoryId = g.Key.CategoryId,
                    CategoryName = g.Key.CategoryName,
                    Revenue = g.Sum(od => od.TotalPrice),
                    OrderCount = g.Select(od => od.OrderId).Distinct().Count()
                })
                .ToList();

            var totalCategoryRevenue = categoryRevenue.Sum(c => c.Revenue);
            foreach (var item in categoryRevenue)
            {
                item.Percentage = totalCategoryRevenue > 0 ? (item.Revenue / totalCategoryRevenue * 100) : 0;
            }
            model.RevenueByCategory = categoryRevenue.OrderByDescending(c => c.Revenue).ToList();

            // Doanh thu theo phương thức thanh toán
            var paymentRevenue = orders
                .GroupBy(o => o.PaymentMethod)
                .Select(g => new RevenueByPaymentMethodViewModel
                {
                    PaymentMethod = g.Key ?? "Chưa xác định",
                    Amount = g.Sum(o => o.FinalAmount),
                    OrderCount = g.Count()
                })
                .ToList();

            var totalPaymentRevenue = paymentRevenue.Sum(p => p.Amount);
            foreach (var item in paymentRevenue)
            {
                item.Percentage = totalPaymentRevenue > 0 ? (item.Amount / totalPaymentRevenue * 100) : 0;
            }
            model.RevenueByPaymentMethod = paymentRevenue.OrderByDescending(p => p.Amount).ToList();

            return View(model);
        }
    }
}
