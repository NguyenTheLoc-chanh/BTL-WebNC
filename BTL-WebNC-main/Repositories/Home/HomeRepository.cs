using BTL_WEBNC.Models;
using BTL_WEBNC.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BTL_WEBNC.Repositories
{
    public class HomeRepository : IHomeRepository
    {
        private readonly AppDBContext _context;

        public HomeRepository(AppDBContext context)
        {
            _context = context;
        }

        // Lấy tất cả các sản phẩm đã được duyệt và trong vòng 7 ngày gần đây
        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await _context.Products
                .Where(p => p.ApprovalStatus == ProductApprovalStatus.Approved)
                .Where(p => p.CreatedAt >= DateTime.UtcNow.AddDays(-6)) // sản phẩm trong 7 ngày gần đây
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        // Lấy sản phẩm theo phân trang (chỉ lấy sản phẩm đã được duyệt)
        public async Task<List<Product>> GetProductsAsync(int page, int pageSize)
        {
            return await _context.Products
                .Where(p => p.ApprovalStatus == ProductApprovalStatus.Approved)
                .OrderByDescending(p => p.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        // Lấy tổng số sản phẩm đã được duyệt
        public async Task<int> GetTotalProductsAsync()
        {
            return await _context.Products
                .CountAsync(p => p.ApprovalStatus == ProductApprovalStatus.Approved);
        }

        // Lấy sản phẩm mới nhất (chỉ lấy sản phẩm đã được duyệt) trong 1 ngày qua
        public async Task<List<Product>> GetLatestProductsAsync()
        {
            DateTime yesterday = DateTime.UtcNow.AddDays(-1);
            return await _context.Products
                .Where(p => p.CreatedAt >= yesterday && p.ApprovalStatus == ProductApprovalStatus.Approved)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        // Lấy sản phẩm theo danh mục (chỉ sản phẩm đã được duyệt)
        public async Task<List<Product>> GetProductsByCategoryAsync(int categoryId)
        {
            return await _context.Products
                .Where(p => p.category_id == categoryId)
                .Where(p => p.ApprovalStatus == ProductApprovalStatus.Approved)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        // Lấy sản phẩm theo giá, chỉ áp dụng cho sản phẩm đã được duyệt
        public List<Product> GetProductsByPriceAsync(string order)
        {
            var products = _context.Products
                .Where(p => p.ApprovalStatus == ProductApprovalStatus.Approved)
                .AsQueryable();

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

        // Tìm sản phẩm theo từ khóa (chỉ lấy sản phẩm đã được duyệt)
        public List<Product> SearchProductsAsync(string keyword)
        {
            return _context.Products
                .Where(p => p.ApprovalStatus == ProductApprovalStatus.Approved)
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

        // Hàm lấy chi tiết sản phẩm (chỉ lấy khi sản phẩm đã được duyệt)
        public async Task<Product?> GetProductByIdAsync(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Seller)
                .FirstOrDefaultAsync(p => p.Product_Id == id && p.ApprovalStatus == ProductApprovalStatus.Approved);
            if (product == null)
            {
                Console.WriteLine($"Không tìm thấy sản phẩm với ID = {id}");
            }
            return product;
        }

        // Lấy danh sách kích cỡ của sản phẩm
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

        // Lấy một số sản phẩm của nhà bán dựa vào từ khóa, chỉ trả về sản phẩm đã được duyệt
        public async Task<List<Product>> GetProductBySelerIdAsync(int sellerID, string keywords)
        {
            var keywordList = keywords.Split(' ');
            return await _context.Products
                .Where(p => p.seller_Id == sellerID && keywordList.Any(k => p.Name.Contains(k)))
                .Where(p => p.ApprovalStatus == ProductApprovalStatus.Approved)
                .OrderByDescending(p => p.CreatedAt)
                .Take(5)
                .ToListAsync();
        }

        // Lấy thông tin của nhà bán theo ID
        public async Task<Seller?> GetSellerByIDAsync(int seller_Id)
        {
            return await _context.Sellers
                .Where(s => s.seller_Id == seller_Id)
                .FirstOrDefaultAsync();
        }
    }
}
