using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Exe_Demo.Data;
using Exe_Demo.Models;

namespace Exe_Demo.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProductController> _logger;

        public ProductController(ApplicationDbContext context, ILogger<ProductController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Product
        public async Task<IActionResult> Index(int? categoryId, string? search, string? sortBy)
        {
            var productsQuery = _context.Products
                .Include(p => p.Category)
                .Where(p => p.IsActive == true);

            // Filter by category
            if (categoryId.HasValue && categoryId.Value > 0)
            {
                productsQuery = productsQuery.Where(p => p.CategoryId == categoryId.Value);
            }

            // Search
            if (!string.IsNullOrEmpty(search))
            {
                productsQuery = productsQuery.Where(p => 
                    p.ProductName.Contains(search) || 
                    p.Description.Contains(search));
            }

            // Sort
            productsQuery = sortBy switch
            {
                "price_asc" => productsQuery.OrderBy(p => p.Price),
                "price_desc" => productsQuery.OrderByDescending(p => p.Price),
                "name" => productsQuery.OrderBy(p => p.ProductName),
                "newest" => productsQuery.OrderByDescending(p => p.CreatedDate),
                _ => productsQuery.OrderBy(p => p.ProductId)
            };

            var products = await productsQuery.ToListAsync();
            var categories = await _context.Categories
                .Where(c => c.IsActive == true)
                .OrderBy(c => c.DisplayOrder)
                .ToListAsync();

            ViewBag.Categories = categories;
            ViewBag.CurrentCategory = categoryId;
            ViewBag.CurrentSearch = search;
            ViewBag.CurrentSort = sortBy;

            return View(products);
        }

        // GET: Product/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Reviews)
                .FirstOrDefaultAsync(p => p.ProductId == id);

            if (product == null)
            {
                return NotFound();
            }

            // Increment view count
            product.ViewCount = (product.ViewCount ?? 0) + 1;
            await _context.SaveChangesAsync();

            // Get related products (same category)
            var relatedProducts = await _context.Products
                .Where(p => p.CategoryId == product.CategoryId && p.ProductId != id && p.IsActive == true)
                .Take(4)
                .ToListAsync();

            ViewBag.RelatedProducts = relatedProducts;

            return View(product);
        }

        // GET: Product/Category/1
        public async Task<IActionResult> Category(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index), new { categoryId = id });
        }
    }
}
