using BTL_WEBNC.Models;

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
    }
}
