using Exe_Demo.Data;
using Exe_Demo.Models;
using Microsoft.EntityFrameworkCore;

namespace Exe_Demo.Database
{
    public static class SeedData
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            try
            {
                // Ensure database is created
                await context.Database.EnsureCreatedAsync();

                // Check if data already exists
                if (await context.Products.AnyAsync())
                {
                    Console.WriteLine("Database already seeded");
                    return;
                }

                Console.WriteLine("Seeding database...");

                // Seed Categories
                var categories = new List<Category>
                {
                    new Category { CategoryId = 1, CategoryName = "Sản phẩm sấy dẻo (200g)", Description = "Hoa quả sấy dẻo 200g", IsActive = true },
                    new Category { CategoryId = 2, CategoryName = "Sản phẩm sấy giòn (200g)", Description = "Hoa quả sấy giòn 200g", IsActive = true },
                    new Category { CategoryId = 3, CategoryName = "Sản phẩm sấy thăng hoa (100g)", Description = "Hoa quả sấy thăng hoa 100g", IsActive = true },
                    new Category { CategoryId = 4, CategoryName = "Mini size mix (50g)", Description = "Hoa quả sấy mini 50g", IsActive = true }
                };

                await context.Categories.AddRangeAsync(categories);

                // Seed Products
                var products = new List<Product>
                {
                    // Sấy dẻo (200g)
                    new Product
                    {
                        ProductId = 1,
                        ProductCode = "SD-MAN-200",
                        ProductName = "Mận Sấy Dẻo",
                        CategoryId = 1,
                        Description = "Mận sấy dẻo Mộc Châu được chế biến từ những trái mận chín mọng, tươi ngon. Sản phẩm giữ nguyên vị chua ngọt tự nhiên, mềm mại, thơm ngon. Giàu vitamin C, chất xơ tốt cho sức khỏe.",
                        ShortDescription = "Mận sấy dẻo Mộc Châu, vị chua ngọt tự nhiên",
                        Price = 65000,
                        OriginalPrice = 75000,
                        StockQuantity = 100,
                        Unit = "Gói",
                        Weight = "200g",
                        ImageUrl = "/images/products/man-say-deo.jpg",
                        IsActive = true,
                        IsFeatured = true,
                        IsNew = false,
                        ViewCount = 0,
                        SoldCount = 0,
                        Rating = 4.5m,
                        CreatedDate = DateTime.Now
                    },
                    new Product
                    {
                        ProductId = 2,
                        ProductCode = "SD-XOAI-200",
                        ProductName = "Xoài Sấy Dẻo",
                        CategoryId = 1,
                        Description = "Xoài sấy dẻo Mộc Châu từ xoài chín tự nhiên, ngọt thanh, mềm mại. Giữ nguyên hương vị đặc trưng của xoài tươi, bổ sung vitamin A, C và chất xơ.",
                        ShortDescription = "Xoài sấy dẻo Mộc Châu, ngọt thanh tự nhiên",
                        Price = 70000,
                        OriginalPrice = 80000,
                        StockQuantity = 100,
                        Unit = "Gói",
                        Weight = "200g",
                        ImageUrl = "/images/products/xoai-say-deo.jpg",
                        IsActive = true,
                        IsFeatured = true,
                        IsNew = false,
                        ViewCount = 0,
                        SoldCount = 0,
                        Rating = 4.7m,
                        CreatedDate = DateTime.Now
                    },
                    new Product
                    {
                        ProductId = 3,
                        ProductCode = "SD-DAO-200",
                        ProductName = "Đào Sấy Dẻo",
                        CategoryId = 1,
                        Description = "Đào sấy dẻo Mộc Châu từ đào tươi ngon, giữ nguyên vị ngọt đậm đà. Sản phẩm mềm mại, thơm ngon, giàu vitamin và khoáng chất tốt cho sức khỏe.",
                        ShortDescription = "Đào sấy dẻo Mộc Châu, ngọt đậm đà",
                        Price = 65000,
                        OriginalPrice = 75000,
                        StockQuantity = 100,
                        Unit = "Gói",
                        Weight = "200g",
                        ImageUrl = "/images/products/dao-say-deo.jpg",
                        IsActive = true,
                        IsFeatured = false,
                        IsNew = true,
                        ViewCount = 0,
                        SoldCount = 0,
                        Rating = 4.3m,
                        CreatedDate = DateTime.Now
                    },
                    new Product
                    {
                        ProductId = 4,
                        ProductCode = "SD-DAU-200",
                        ProductName = "Dâu Sấy Dẻo",
                        CategoryId = 1,
                        Description = "Dâu sấy dẻo Mộc Châu từ dâu tây tươi ngon, vị chua ngọt hài hòa. Giàu vitamin C, chất chống oxy hóa, tốt cho làn da và sức khỏe tim mạch.",
                        ShortDescription = "Dâu sấy dẻo Mộc Châu, chua ngọt hài hòa",
                        Price = 90000,
                        OriginalPrice = 100000,
                        StockQuantity = 100,
                        Unit = "Gói",
                        Weight = "200g",
                        ImageUrl = "/images/products/dau-say-deo.jpg",
                        IsActive = true,
                        IsFeatured = true,
                        IsNew = false,
                        ViewCount = 0,
                        SoldCount = 0,
                        Rating = 4.8m,
                        CreatedDate = DateTime.Now
                    },
                    new Product
                    {
                        ProductId = 5,
                        ProductCode = "SD-HONG-200",
                        ProductName = "Hồng Sấy Dẻo",
                        CategoryId = 1,
                        Description = "Hồng sấy dẻo Mộc Châu từ hồng chín tự nhiên, ngọt thanh, mềm mại. Giàu vitamin A, C và chất xơ, tốt cho tiêu hóa và làm đẹp da.",
                        ShortDescription = "Hồng sấy dẻo Mộc Châu, ngọt thanh mềm mại",
                        Price = 95000,
                        OriginalPrice = 110000,
                        StockQuantity = 100,
                        Unit = "Gói",
                        Weight = "200g",
                        ImageUrl = "/images/products/hong-say-deo.jpg",
                        IsActive = true,
                        IsFeatured = false,
                        IsNew = true,
                        ViewCount = 0,
                        SoldCount = 0,
                        Rating = 4.6m,
                        CreatedDate = DateTime.Now
                    },
                    // Sấy giòn (200g)
                    new Product
                    {
                        ProductId = 6,
                        ProductCode = "SG-MIT-200",
                        ProductName = "Mít Sấy Giòn",
                        CategoryId = 2,
                        Description = "Mít sấy giòn Mộc Châu từ mít tươi ngon, giòn tan, thơm ngọt. Công nghệ sấy hiện đại giữ nguyên hương vị đặc trưng, giàu chất xơ và vitamin.",
                        ShortDescription = "Mít sấy giòn Mộc Châu, giòn tan thơm ngọt",
                        Price = 80000,
                        OriginalPrice = 90000,
                        StockQuantity = 100,
                        Unit = "Gói",
                        Weight = "200g",
                        ImageUrl = "/images/products/mit-say-gion.jpg",
                        IsActive = true,
                        IsFeatured = true,
                        IsNew = false,
                        ViewCount = 0,
                        SoldCount = 0,
                        Rating = 4.4m,
                        CreatedDate = DateTime.Now
                    },
                    new Product
                    {
                        ProductId = 7,
                        ProductCode = "SG-CHUOI-200",
                        ProductName = "Chuối Sấy Giòn",
                        CategoryId = 2,
                        Description = "Chuối sấy giòn Mộc Châu từ chuối chín vàng, giòn rụm, ngọt tự nhiên. Giàu kali, magie và vitamin B6, tốt cho tim mạch và hệ thần kinh.",
                        ShortDescription = "Chuối sấy giòn Mộc Châu, giòn rụm ngọt tự nhiên",
                        Price = 80000,
                        OriginalPrice = 90000,
                        StockQuantity = 100,
                        Unit = "Gói",
                        Weight = "200g",
                        ImageUrl = "/images/products/chuoi-say-gion.jpg",
                        IsActive = true,
                        IsFeatured = false,
                        IsNew = false,
                        ViewCount = 0,
                        SoldCount = 0,
                        Rating = 4.2m,
                        CreatedDate = DateTime.Now
                    }
                };

                await context.Products.AddRangeAsync(products);
                await context.SaveChangesAsync();

                Console.WriteLine("Database seeded successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error seeding database: {ex.Message}");
                throw;
            }
        }
    }
}
