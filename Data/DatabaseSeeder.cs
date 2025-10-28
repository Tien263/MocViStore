using Exe_Demo.Models;
using Microsoft.EntityFrameworkCore;

namespace Exe_Demo.Data;

public static class DatabaseSeeder
{
    public static void SeedData(ApplicationDbContext context)
    {
        // Check if data already exists
        if (context.Categories.Any())
        {
            Console.WriteLine("Database already seeded");
            return;
        }

        Console.WriteLine("Seeding database...");

        // Seed Categories
        var categories = new List<Category>
        {
            new Category { CategoryId = 1, CategoryName = "Hoa Quả Sấy", Description = "Các loại hoa quả sấy khô tự nhiên" },
            new Category { CategoryId = 2, CategoryName = "Hoa Quả Sấy Dẻo", Description = "Hoa quả sấy giữ độ mềm tự nhiên" },
            new Category { CategoryId = 3, CategoryName = "Hoa Quả Sấy Thăng Hoa", Description = "Hoa quả sấy công nghệ thăng hoa" },
            new Category { CategoryId = 4, CategoryName = "Combo Quà Tặng", Description = "Combo hoa quả sấy làm quà" }
        };
        context.Categories.AddRange(categories);

        // Seed Products
        var products = new List<Product>
        {
            new Product
            {
                ProductId = 1,
                ProductName = "Mít Sấy Giòn",
                CategoryId = 1,
                Price = 150000,
                StockQuantity = 100,
                Description = "Mít sấy giòn tự nhiên, không chất bảo quản",
                ImageUrl = "/images/products/mit-say.jpg",
                IsActive = true
            },
            new Product
            {
                ProductId = 2,
                ProductName = "Chuối Sấy Dẻo",
                CategoryId = 2,
                Price = 120000,
                StockQuantity = 150,
                Description = "Chuối sấy dẻo thơm ngon, giữ nguyên vị tự nhiên",
                ImageUrl = "/images/products/chuoi-say.jpg",
                IsActive = true
            },
            new Product
            {
                ProductId = 3,
                ProductName = "Xoài Sấy Dẻo",
                CategoryId = 2,
                Price = 180000,
                StockQuantity = 80,
                Description = "Xoài sấy dẻo chua ngọt đậm đà",
                ImageUrl = "/images/products/xoai-say.jpg",
                IsActive = true
            },
            new Product
            {
                ProductId = 4,
                ProductName = "Dâu Tây Sấy Thăng Hoa",
                CategoryId = 3,
                Price = 250000,
                StockQuantity = 50,
                Description = "Dâu tây sấy thăng hoa giữ nguyên hương vị",
                ImageUrl = "/images/products/dau-say.jpg",
                IsActive = true
            },
            new Product
            {
                ProductId = 5,
                ProductName = "Combo Hoa Quả Sấy 5 Loại",
                CategoryId = 4,
                Price = 350000,
                StockQuantity = 30,
                Description = "Combo 5 loại hoa quả sấy đa dạng",
                ImageUrl = "/images/products/combo-5.jpg",
                IsActive = true
            }
        };
        context.Products.AddRange(products);

        // Seed Admin User
        var admin = new User
        {
            UserId = 1,
            Username = "admin",
            Email = "admin@mocvistore.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
            FullName = "Administrator",
            Role = "Admin",
            IsActive = true,
            CreatedAt = DateTime.Now
        };
        context.Users.Add(admin);

        context.SaveChanges();
        Console.WriteLine("Database seeded successfully!");
    }
}
