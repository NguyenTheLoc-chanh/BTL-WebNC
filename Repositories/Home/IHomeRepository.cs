using BTL_WEBNC.Models;

namespace BTL_WEBNC.Repositories
{
    public interface IHomeRepository
    {
        // Lấy ra sản phẩm 2 ngày gần nhất cho trang Home
        Task<List<Product>> GetAllProductsAsync();
        Task<List<Product>> GetProductsAsync(int page, int pageSize);
        Task<int> GetTotalProductsAsync();
        Task<List<Product>> GetLatestProductsAsync();
        Task<List<Product>> GetProductsByCategoryAsync(int categoryId);
        List<Product> GetProductsByPriceAsync(string order);
        List<Product> SearchProductsAsync(string keyword);
    }
}
