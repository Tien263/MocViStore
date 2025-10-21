using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Exe_Demo.Data;
using Exe_Demo.Models;

namespace Exe_Demo.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CartController> _logger;

        public CartController(ApplicationDbContext context, ILogger<CartController> logger)
        {
            _context = context;
            _logger = logger;
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

        public IActionResult Checkout()
        {
            return View();
        }
    }
}
