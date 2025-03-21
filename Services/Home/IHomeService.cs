using BTL_WEBNC.Models;
using BTL_WEBNC.Models.ViewModels;

namespace BTL_WEBNC.Services
{
    public interface IHomeService
    {
        Task<List<Product>> GetAllProductsAsync();
        Task<(List<Product>, int, int)> GetPagedProductsAsync(int page, int pageSize);
        Task<List<Product>> GetLatestProductsAsync();
        Task<List<Product>> GetProductsByCategoryAsync(int categoryId);
        List<Product> GetProductsByPriceAsync(string order);
        List<Product> SearchProductsAsync(string keyword);

        Task<Product?> GetProductByIdAsync(int productId);
        Task<List<ProductSizeViewModel>> GetSizesByProductIdAsync(int productId);
        Task<List<Product>> GetProductBySelerIdAsync(int sellerID, string keywords);
    }
}
