using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Exe_Demo.Data;
using Exe_Demo.Models;
using Exe_Demo.Helpers;
using Exe_Demo.Services;

namespace Exe_Demo.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CartController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;

        public CartController(ApplicationDbContext context, ILogger<CartController> logger, IConfiguration configuration, IEmailService emailService)
        {
            _context = context;
            _logger = logger;
            _configuration = configuration;
            _emailService = emailService;
        }

        public async Task<IActionResult> Index()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return RedirectToAction("Login", "Auth");
            }

            // Get customer ID from user
            var user = await _context.Users.FindAsync(userId);
            if (user == null || user.CustomerId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var cartItems = await _context.Carts
                .Include(c => c.Product)
                .ThenInclude(p => p.Category)
                .Where(c => c.CustomerId == user.CustomerId)
                .ToListAsync();

            return View(cartItems);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity = 1)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Json(new { success = false, message = "Vui lòng đăng nhập!" });
            }

            // Get customer ID from user
            var user = await _context.Users.FindAsync(userId);
            if (user == null || user.CustomerId == null)
            {
                return Json(new { success = false, message = "Vui lòng đăng nhập!" });
            }

            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                return Json(new { success = false, message = "Sản phẩm không tồn tại!" });
            }

            // Check if product already in cart
            var existingCart = await _context.Carts
                .FirstOrDefaultAsync(c => c.CustomerId == user.CustomerId && c.ProductId == productId);

            if (existingCart != null)
            {
                // Update quantity
                existingCart.Quantity += quantity;
            }
            else
            {
                // Add new cart item
                var cart = new Cart
                {
                    CustomerId = user.CustomerId,
                    ProductId = productId,
                    Quantity = quantity,
                    CreatedDate = DateTime.Now
                };
                _context.Carts.Add(cart);
            }

            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Đã thêm vào giỏ hàng!" });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(int cartId, int quantity)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Json(new { success = false, message = "Vui lòng đăng nhập!" });
            }

            var user = await _context.Users.FindAsync(userId);
            if (user == null || user.CustomerId == null)
            {
                return Json(new { success = false, message = "Vui lòng đăng nhập!" });
            }

            var cart = await _context.Carts
                .FirstOrDefaultAsync(c => c.CartId == cartId && c.CustomerId == user.CustomerId);

            if (cart == null)
            {
                return Json(new { success = false, message = "Không tìm thấy sản phẩm trong giỏ hàng!" });
            }

            if (quantity <= 0)
            {
                _context.Carts.Remove(cart);
            }
            else
            {
                cart.Quantity = quantity;
            }

            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int cartId)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Json(new { success = false, message = "Vui lòng đăng nhập!" });
            }

            var user = await _context.Users.FindAsync(userId);
            if (user == null || user.CustomerId == null)
            {
                return Json(new { success = false, message = "Vui lòng đăng nhập!" });
            }

            var cart = await _context.Carts
                .FirstOrDefaultAsync(c => c.CartId == cartId && c.CustomerId == user.CustomerId);

            if (cart == null)
            {
                return Json(new { success = false, message = "Không tìm thấy sản phẩm trong giỏ hàng!" });
            }

            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }

        public async Task<IActionResult> GetCartCount()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Json(0);
            }

            var user = await _context.Users.FindAsync(userId);
            if (user == null || user.CustomerId == null)
            {
                return Json(0);
            }

            var count = await _context.Carts
                .Where(c => c.CustomerId == user.CustomerId)
                .SumAsync(c => c.Quantity);

            return Json(count);
        }

        public async Task<IActionResult> Checkout()
        {
            // Kiểm tra đăng nhập
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                TempData["ErrorMessage"] = "Vui lòng đăng nhập để đặt hàng!";
                return RedirectToAction("Login", "Auth", new { returnUrl = Url.Action("Checkout", "Cart") });
            }

            var user = await _context.Users.FindAsync(userId);
            if (user == null || user.CustomerId == null)
            {
                TempData["ErrorMessage"] = "Vui lòng đăng nhập để đặt hàng!";
                return RedirectToAction("Login", "Auth", new { returnUrl = Url.Action("Checkout", "Cart") });
            }

            // Lấy thông tin customer
            var customer = await _context.Customers.FindAsync(user.CustomerId);
            if (customer == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy thông tin khách hàng!";
                return RedirectToAction("Index", "Cart");
            }

            // Lấy giỏ hàng
            var cartItems = await _context.Carts
                .Include(c => c.Product)
                .ThenInclude(p => p.Category)
                .Where(c => c.CustomerId == user.CustomerId)
                .ToListAsync();

            if (!cartItems.Any())
            {
                TempData["ErrorMessage"] = "Giỏ hàng của bạn đang trống!";
                return RedirectToAction("Index", "Cart");
            }

            // Truyền dữ liệu sang view
            ViewBag.Customer = customer;
            ViewBag.CartItems = cartItems;
            ViewBag.Total = cartItems.Sum(c => c.Quantity * c.Product.Price);

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PlaceOrder(string FullName, string Phone, string Email, string Address, string Note, string PaymentMethod)
        {
            // Kiểm tra đăng nhập
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                TempData["ErrorMessage"] = "Vui lòng đăng nhập để đặt hàng!";
                return RedirectToAction("Login", "Auth");
            }

            var user = await _context.Users.FindAsync(userId);
            if (user == null || user.CustomerId == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy thông tin khách hàng!";
                return RedirectToAction("Index", "Cart");
            }

            // Lấy giỏ hàng
            var cartItems = await _context.Carts
                .Include(c => c.Product)
                .Where(c => c.CustomerId == user.CustomerId)
                .ToListAsync();

            if (!cartItems.Any())
            {
                TempData["ErrorMessage"] = "Giỏ hàng của bạn đang trống!";
                return RedirectToAction("Index", "Cart");
            }

            // Tính tổng tiền
            decimal totalAmount = cartItems.Sum(c => c.Quantity * c.Product.Price);

            // Tạo đơn hàng
            var order = new Models.Order
            {
                OrderCode = "ORD" + DateTime.Now.Ticks.ToString().Substring(0, 10),
                CustomerId = user.CustomerId,
                CustomerName = FullName,
                CustomerEmail = Email,
                CustomerPhone = Phone,
                ShippingAddress = Address,
                TotalAmount = totalAmount,
                FinalAmount = totalAmount,
                PaymentMethod = PaymentMethod,
                PaymentStatus = PaymentMethod == "COD" ? "Pending" : "Pending",
                OrderStatus = "Pending",
                Note = Note,
                CreatedDate = DateTime.Now
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Tạo chi tiết đơn hàng
            foreach (var item in cartItems)
            {
                var orderDetail = new Models.OrderDetail
                {
                    OrderId = order.OrderId,
                    ProductId = item.ProductId,
                    ProductName = item.Product.ProductName,
                    Quantity = item.Quantity,
                    Price = item.Product.Price,
                    DiscountPercent = item.Product.DiscountPercent ?? 0,
                    TotalPrice = item.Quantity * item.Product.Price
                };
                _context.OrderDetails.Add(orderDetail);
            }

            // Xóa giỏ hàng
            _context.Carts.RemoveRange(cartItems);
            await _context.SaveChangesAsync();

            // Load lại order với OrderDetails để gửi email
            var orderWithDetails = await _context.Orders
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(o => o.OrderId == order.OrderId);

            // Gửi email xác nhận đơn hàng
            if (orderWithDetails != null)
            {
                try
                {
                    await _emailService.SendOrderConfirmationEmailAsync(orderWithDetails);
                    _logger.LogInformation($"Email xác nhận đơn hàng #{orderWithDetails.OrderCode} đã được gửi đến {orderWithDetails.CustomerEmail}");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Lỗi gửi email xác nhận đơn hàng: {ex.Message}");
                    // Không throw exception để không ảnh hưởng đến quá trình đặt hàng
                }
            }

            // Xử lý theo phương thức thanh toán
            if (PaymentMethod == "Bank")
            {
                // Chuyển hướng đến trang hiển thị thông tin chuyển khoản
                return RedirectToAction("BankTransferInfo", new { orderId = order.OrderId });
            }
            else if (PaymentMethod == "COD")
            {
                TempData["SuccessMessage"] = $"Đặt hàng thành công! Mã đơn hàng: #{order.OrderCode}. Chúng tôi sẽ liên hệ với bạn sớm nhất. Thông tin chi tiết đã được gửi qua email.";
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> BankTransferInfo(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy đơn hàng!";
                return RedirectToAction("Index", "Home");
            }

            // Truyền thông tin ngân hàng từ appsettings.json
            ViewBag.BankName = _configuration["BankTransfer:BankName"];
            ViewBag.AccountNumber = _configuration["BankTransfer:AccountNumber"];
            ViewBag.AccountName = _configuration["BankTransfer:AccountName"];
            ViewBag.BankBranch = _configuration["BankTransfer:BankBranch"];

            return View(order);
        }
    }
}
