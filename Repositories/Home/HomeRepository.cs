using BTL_WEBNC.Models;
using BTL_WEBNC.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace BTL_WEBNC.Repositories
{
    public class HomeRepository : IHomeRepository
    {
        private readonly AppDBContext _context;

        public HomeRepository(AppDBContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetAllProductsAsync(){
            return await _context.Products
                .Where(p => p.CreatedAt >= DateTime.UtcNow.AddDays(-6)) // Sản phẩm trong vòng 7 ngày
                .OrderByDescending(p => p.CreatedAt) // Sắp xếp từ mới nhất -> cũ nhất
                .ToListAsync();
        }

        public async Task<List<Product>> GetProductsAsync(int page, int pageSize)
        {
            return await _context.Products
                .OrderByDescending(p => p.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetTotalProductsAsync()
        {
            return await _context.Products.CountAsync();
        }
        
        public async Task<List<Product>> GetLatestProductsAsync()
        {
            DateTime yesterday = DateTime.UtcNow.AddDays(-1);
            return await _context.Products
                .Where(p => p.CreatedAt >= yesterday)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Product>> GetProductsByCategoryAsync(int categoryId)
        {
            return await _context.Products
                .Where(p => p.category_id == categoryId) // Lọc theo danh mục
                .OrderByDescending(p => p.CreatedAt) // Sắp xếp sản phẩm mới nhất
                .ToListAsync();
        }

        public List<Product> GetProductsByPriceAsync(string order)
        {
            var products = _context.Products.AsQueryable();

            if (order == "asc")
            {
                products = products.OrderBy(p => p.Price);
            }
            else if (order == "desc")
            {
                products = products.OrderByDescending(p => p.Price);
            }
            return products.ToList();
        }

        public List<Product> SearchProductsAsync(string keyword)
        {
            return _context.Products
                .Where(p => p.Name.Contains(keyword))
                .Select(p => new Product
                {
                    Product_Id = p.Product_Id,
                    Name = p.Name,
                    Price = p.Price,
                    Images = p.Images
                })
                .ToList();
        }

        // Hàm lấy chi tiết sản phẩm
        public async Task<Product?> GetProductByIdAsync(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Product_Id == id);
            if (product == null)
            {
                Console.WriteLine($"Không tìm thấy sản phẩm với ID = {id}");
            }
            return product;
        }

        public async Task<List<ProductSizeViewModel>> GetSizesByProductIdAsync(int productId)
        {
            return await _context.ProductSizes
                .Where(ps => ps.Product_Id == productId)
                .Select(ps => new ProductSizeViewModel
                {
                    Size_Id = ps.Size.Size_Id,
                    Name = ps.Size.Name,
                    Stock = ps.Stock
                })
                .ToListAsync();
        }

        public async Task<List<Product>> GetProductBySelerIdAsync(int sellerID, string keywords)
        {
             var keywordList = keywords.Split(' ');
            return await _context.Products
                .Where(p => p.seller_Id == sellerID && keywordList.Any(k => p.Name.Contains(k)))
                .OrderByDescending(p => p.CreatedAt)
                .Take(5)
                .ToListAsync(); 
        }
    }
}